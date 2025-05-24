using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VotingSystem.DataAccess;
using VotingSystem.DataAccess.Models;
using VotingSystem.Shared.Models;
using Program = VotingSystem.WebAPI.Program;

namespace VotingSystem.Tests.IntegrationTests;

public class VotesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    
    private static readonly LoginRequestDto UserLogin = new()
    {
        Email = "user2@gmail.com",
        Password = "Password.123"
    };
    
    public VotesControllerIntegrationTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the real database with an in-memory database
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<VotingSystemDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<VotingSystemDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestVotesDatabase");
                });

                //Seed the database with initial data
                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<VotingSystemDbContext>();
                db.Database.EnsureCreated();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
                SeedRoles(roleManager);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                SeedUsers(userManager);
                
                SeedVotes(db, userManager.Users.ToList());
            });
        });

        _client = _factory.CreateClient();
    }

    #region Get
    
    [Fact]
    public async Task GetVotes_ReturnsAllVotes()
    {
        await Login(UserLogin);
        
        // Act
        var response = await _client.GetAsync("/votes");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var votes = await response.Content.ReadFromJsonAsync<List<VoteResponseDto>>();
        Assert.NotNull(votes);
        Assert.True(votes.Count == 4);
    }
    
    [Fact]
    public async Task GetActiveVotes_ReturnsAllActiveVotes()
    {
        await Login(UserLogin);
        
        // Act
        var response = await _client.GetAsync("/votes/active");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var votes = await response.Content.ReadFromJsonAsync<List<VoteResponseDto>>();
        Assert.NotNull(votes);
        Assert.Equal(3, votes.Count);
    }
    
    [Fact]
    public async Task GetClosedVotes_ReturnsAllClosedVotes()
    {
        await Login(UserLogin);
        
        // Act
        var response = await _client.GetAsync("/votes/closed");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var votes = await response.Content.ReadFromJsonAsync<List<VoteResponseDto>>();
        Assert.NotNull(votes);
        Assert.True(votes.Count == 1);
    }
    
    [Fact]
         public async Task GetVoteById_ReturnsVote_WhenVoteExists()
         {
             await Login(UserLogin);
             
             // Arrange
             const int voteId = 2;
     
             // Act
             var response = await _client.GetAsync($"/votes/{voteId}");
     
             // Assert
             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
             var vote = await response.Content.ReadFromJsonAsync<VoteResponseDto>();
             Assert.NotNull(vote);
             Assert.Equal(voteId, vote.Id);
             Assert.Equal("What is the meaning of life?", vote.Question);
         }
     
         [Fact]
         public async Task GetVoteById_ReturnsNotFound_WhenVoteNotExists()
         {
             await Login(UserLogin);
             
             // Arrange
             var voteId = 99; // ID of a seeded vote
     
             // Act
             var response = await _client.GetAsync($"/votes/{voteId}");
     
             // Assert
             Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
             var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
             Assert.NotNull(problemDetails);
         }
         
         [Fact]
         public async Task GetVoteResultsById_ReturnsVoteResults_WhenVoteExists()
         {
             await Login(UserLogin);
             
             // Arrange
             const int voteId = 2;
     
             // Act
             var response = await _client.GetAsync($"/votes/{voteId}/results");
     
             // Assert
             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
             using var jsonDoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
             var resultsElement = jsonDoc.RootElement.GetProperty("results");
             var vote = JsonSerializer.Deserialize<Dictionary<string, int>>(resultsElement.GetRawText());
             
             Assert.NotNull(vote);
             Assert.True(vote.ContainsKey("Nothing"));
             Assert.Equal(2, vote["42"]);
             Assert.Equal(1, vote["IDK"]);
         }
         
         [Fact]
         public async Task GetVoteResultsById_ReturnsBadRequest_WhenVoteStillOpen()
         {
             await Login(UserLogin);
             
             // Arrange
             const int voteId = 1; // ID of a seeded vote
     
             // Act
             var response = await _client.GetAsync($"/votes/{voteId}/results");
     
             // Assert
             Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
             var problemDetails = await response.Content.ReadFromJsonAsync<string>();
             Assert.NotNull(problemDetails);
         }
     
         [Fact]
         public async Task GetVoteResultsById_ReturnsNotFound_WhenVoteNotExists()
         {
             await Login(UserLogin);
             
             // Arrange
             var voteId = 99; // ID of a seeded vote
     
             // Act
             var response = await _client.GetAsync($"/votes/{voteId}/results");
     
             // Assert
             Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
             var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
             Assert.NotNull(problemDetails);
         }
         
         [Fact]
         public async Task GetUserVotedByVoteId_ReturnsNotFound_WhenUserNotExists()
         {
             await Login(UserLogin);
             
             // Arrange
             const int voteId = 2;
             const string userId = "100";
     
             // Act
             var response = await _client.GetAsync($"/votes/voted/{voteId}/{userId}");
     
             // Assert
             var problemDetails = await response.Content.ReadFromJsonAsync<bool>();
             Assert.False(problemDetails);
         }
         
         [Fact]
         public async Task GetUserVotedByVoteId_ReturnsNotFound_WhenVoteNotExists()
         {
             await Login(UserLogin);
             // Arrange
             const int voteId = 99;
             const string userId = "2a549ec8-85a6-426a-9e0c-d5d795608951";
     
             // Act
             var response = await _client.GetAsync($"/votes/voted/{voteId}/{userId}");
     
             // Assert
             Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
             var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
             Assert.NotNull(problemDetails);
         }
         
         [Fact]
         public async Task GetUserVotedByVoteId_ReturnsNotFound_WhenVoteAndUserNotExists()
         {
             await Login(UserLogin);
             // Arrange
             const int voteId = 99;
             const string userId = "100" ;
     
             // Act
             var response = await _client.GetAsync($"/votes/voted/{voteId}/{userId}");
     
             // Assert
             Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
             var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
             Assert.NotNull(problemDetails);
         }
    
    
    #endregion
    
    #region Create

    [Fact]
    public async Task CreateVote_ReturnsBadRequest_WhenOptionsAreInvalid()
    {
        // Arrange
        var invalidVote = new VoteRequestDto
        {
            Question = "Will this test work?",
            Options = ["Yes"],
            Start = DateTime.Now + TimeSpan.FromHours(1),
            End = DateTime.Now + TimeSpan.FromHours(10)
        };

        // Act
        await Login(UserLogin);
        var response = await _client.PostAsJsonAsync("/votes", invalidVote);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails);
    }

    [Fact]
    public async Task CreateVote_ReturnsConflict_WhenQuestionExists()
    {
        // Arrange
        var newVote = new VoteRequestDto
        {
            Question = "What is the capital of the USA?",
            Options = ["Yes", "No", "Maybe"],
            Start = DateTime.Now + TimeSpan.FromHours(1),
            End = DateTime.Now + TimeSpan.FromHours(10)
        };

        // Act
        await Login(UserLogin);
        var response = await _client.PostAsJsonAsync("/votes", newVote);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
    }

    [Fact]
    public async Task CreateVote_AddsVote_WhenDataIsValid()
    {
        // Arrange
        var newVote = new VoteRequestDto
        {
            Question = "Will this test work?",
            Options = ["Yes", "No", "Maybe"],
            Start = DateTime.Now + TimeSpan.FromHours(1),
            End = DateTime.Now + TimeSpan.FromHours(10)
        };


        // Act
        await Login(UserLogin);
        var response = await _client.PostAsJsonAsync("/votes", newVote);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdVote = await response.Content.ReadFromJsonAsync<VoteResponseDto>();
        Assert.NotNull(createdVote);
        Assert.Equal(newVote.Question, createdVote.Question);
        Assert.Equal(newVote.Options, createdVote.Options);
    }
    
    [Fact]
    public async Task GetMyVotes_ReturnsVotesOfUser()
    {
        await Login(UserLogin);

        var email = UserLogin.Email;

        var response = await _client.GetAsync("/votes/my");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var votes = await response.Content.ReadFromJsonAsync<List<VoteResponseDto>>();

        Assert.NotNull(votes);
        Assert.Equal("What is the meaning of life?", votes[0].Question);
    }

    [Fact]
    public async Task GetVoteBySubString_ReturnsVotesMatchingSubstring()
    {
        await Login(UserLogin);

        var body = new SearchRequestDto
        {
            Sub = "capital",
            IsActive = true
        };

        var response = await _client.PostAsJsonAsync("/votes/search", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var votes = await response.Content.ReadFromJsonAsync<List<VoteResponseDto>>();

        Assert.NotNull(votes);
        Assert.All(votes, vote => Assert.Contains("capital", vote.Question, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetVoteBySubString_ReturnsBadRequest_WhenSubStringIsEmpty()
    {
        await Login(UserLogin);

        var body = new SearchRequestDto
        {
            Sub = "",
            IsActive = true
        };

        var response = await _client.PostAsJsonAsync("/votes/search", body);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("Search term cannot be empty", message);
    }

    [Fact]
    public async Task GetVoteByDate_ReturnsVotesWithinDateRange()
    {
        await Login(UserLogin);

        var body = new SearchDateRequestDto
        {
            Start = DateTime.Now - TimeSpan.FromDays(1),
            End = DateTime.Now + TimeSpan.FromDays(1),
            IsActive = true
        };

        var response = await _client.PostAsJsonAsync("/votes/search-by-date", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var votes = await response.Content.ReadFromJsonAsync<List<VoteResponseDto>>();

        Assert.NotNull(votes);
        Assert.All(votes, vote =>
        {
            Assert.True(vote.Start >= body.Start);
            Assert.True(vote.End <= body.End);
        });
    }

    #endregion
    
    #region Helpers
    private void SeedVotes(VotingSystemDbContext context, List<User> users)
    {
        context.Votes.AddRange(
            new Vote
            {
                User = users.ElementAt(0),
                Question = "What is 1+1?",
                Options = ["2", "3"],
                Start = DateTime.Now - TimeSpan.FromHours(4),
                End = DateTime.Now + TimeSpan.FromDays(40)
            },
            new Vote
            {
                User = users.ElementAt(1),
                Question = "What is the meaning of life?",
                Options = ["IDK", "42", "Nothing"],
                Start = DateTime.Now - TimeSpan.FromHours(10),
                End = DateTime.Now - TimeSpan.FromHours(2)
            },
            new Vote
            {
                User = users.ElementAt(0),
                Question = "What is the capital of the USA?",
                Options = ["New York", "Los Angeles", "Washington DC", "Chicago"],
                Start = DateTime.Now - TimeSpan.FromHours(10),
                End = DateTime.Now + TimeSpan.FromHours(10)
            },
            new Vote
            {
                UserId = users.ElementAt(2).Id,
                Question = "What is the best web-development framework?",
                Options = ["React", "Laravel", "ASP.NET", "Angular", "Symfony", "None of the above"],
                Start = DateTime.Now - TimeSpan.FromHours(10),
                End = DateTime.Now + TimeSpan.FromDays(10)
            }
        );

        context.VoteParticipations.AddRange(
            new VoteParticipation
            {
                UserId = users.ElementAt(0).Id,
                VoteId = 1,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(1).Id,
                VoteId = 1,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(2).Id,
                VoteId = 1,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(0).Id,
                VoteId = 2,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(1).Id,
                VoteId = 2,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(2).Id,
                VoteId = 2,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(0).Id,
                VoteId = 3,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(1).Id,
                VoteId = 3,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(0).Id,
                VoteId = 4,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(1).Id,
                VoteId = 4,
            },
            new VoteParticipation
            {
                UserId = users.ElementAt(2).Id,
                VoteId = 4,
            }
        );

        context.AnonymousVotes.AddRange(
            new AnonymousVote
            {
                VoteId = 1,
                SelectedOption = "2"
            },
            new AnonymousVote
            {
                VoteId = 3,
                SelectedOption = "New York"
            },
            new AnonymousVote
            {
                VoteId = 3,
                SelectedOption = "Washington DC"
            },
            new AnonymousVote
            {
                VoteId = 2,
                SelectedOption = "Nothing"
            },
            new AnonymousVote
            {
                VoteId = 2,
                SelectedOption = "42"
            },
            new AnonymousVote
            {
                VoteId = 1,
                SelectedOption = "2"
            },
            new AnonymousVote
            {
                VoteId = 1,
                SelectedOption = "3"
            },
            new AnonymousVote
            {
                VoteId = 4,
                SelectedOption = "ASP.NET"
            },
            new AnonymousVote
            {
                VoteId = 4,
                SelectedOption = "Laravel"
            },
            new AnonymousVote
            {
                VoteId = 4,
                SelectedOption = "Laravel"
            },
            new AnonymousVote
            {
                VoteId = 4,
                SelectedOption = "None of the above"
            },
            new AnonymousVote
            {
                VoteId = 2,
                SelectedOption = "42"
            },
            new AnonymousVote
            {
                VoteId = 2,
                SelectedOption = "IDK"
            });

        context.SaveChanges();
    }

    private static void SeedRoles(RoleManager<UserRole> roleManager)
    {
        string[] roleNames = ["Admin"];

        foreach (var roleName in roleNames)
        {
            var roleExist = roleManager.RoleExistsAsync(roleName).Result;
            if (!roleExist)
            {
                // Create the roles and seed them to the database
                roleManager.CreateAsync(new UserRole(roleName)).Wait();
            }
        }
    }

    private static void SeedUsers(UserManager<User> userManager)
    {
        var user1 = userManager.FindByEmailAsync("johndoe@gmail.com").Result;
        if (user1 == null)
        {
            user1 = new User
                { UserName = "johndoe", Email = "johndoe@gmail.com", FirstName = "John", LastName = "Doe" };
            userManager.CreateAsync(user1, "Password.123");
            userManager.AddToRoleAsync(user1, "Admin");
        }

        var user2 = userManager.FindByEmailAsync("user2@gmail.com").Result;
        if (user2 == null)
        {
            user2 = new User { UserName = "user2", Email = "user2@gmail.com", FirstName = "User", LastName = "Two"};
            userManager.CreateAsync(user2, "Password.123");
            userManager.AddToRoleAsync(user2, "Admin");
        }
        
        var user3 = userManager.FindByEmailAsync("test@gmail.com").Result;
        if (user3 == null)
        {
            user3 = new User { UserName = "teszt_elek", Email = "teszt_elek@gmail.com", FirstName = "Teszt", LastName = "Elek"};
            userManager.CreateAsync(user3, "Password.123");
            userManager.AddToRoleAsync(user3, "Admin");
        }
    }

    private async Task Login(LoginRequestDto loginRequest)
    {
        var response = await _client.PostAsJsonAsync("users/login", loginRequest);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.AuthToken);
    }

    public void Dispose()
    {
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<VotingSystemDbContext>();
        db.Database.EnsureDeleted();

        _factory.Dispose();
        _client.Dispose();
    }

    #endregion
}