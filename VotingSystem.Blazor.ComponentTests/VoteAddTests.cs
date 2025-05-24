using AngleSharp.Dom;
using Bunit;
using CloudNimble.EasyAF.Core;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using VotingSystem.Blazor.WebAssembly.Config;
using VotingSystem.Blazor.WebAssembly.Pages.Vote;
using VotingSystem.Blazor.WebAssembly.Services;
using VotingSystem.Blazor.WebAssembly.ViewModels;

namespace VotingSystem.Blazor.ComponentTests;

public class VoteAddTests : IDisposable
{
    private readonly TestContext _context = new();
    private readonly Mock<IVoteService> _voteServiceMock = new();
    private readonly Mock<IJSRuntime> _jsRuntimeMock = new();

    // Test data
    private readonly VoteViewModel _testVote;
    private readonly VoteViewModel _testVote2;

    private readonly List<VoteViewModel> _testVotes = [];

    public VoteAddTests()
    {
        // Setup test data
        _testVote = new()
        {
            Id = 1,
            UserId = "1",
            Question = "Are the tests working?",
            OptionsRaw = "Yes; No; Maybe",
            Start = DateTime.Now - TimeSpan.FromHours(10),
            End = DateTime.Now + TimeSpan.FromHours(10),
            VoteParticipations = []
        };
        
        _testVote2 = new()
        {
            Id = 2,
            UserId = "1",
            Question = "What's your favourite framework?",
            OptionsRaw = "React; Laravel; ASP.NET; Other",
            Start = DateTime.Now - TimeSpan.FromHours(2),
            End = DateTime.Now + TimeSpan.FromDays(7),
            VoteParticipations = []
        };

        _testVotes.Add(_testVote);
        _testVotes.Add(_testVote2);
        
        _voteServiceMock
            .Setup(svc => svc.GetVoteByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => _testVotes.First(v => v.Id == id));
        
        _voteServiceMock.Setup(svc => svc.GetAllUsersAsync())
            .ReturnsAsync(new List<UserViewModel>());
        
        _voteServiceMock
            .Setup(svc => svc.GetMyVotesAsync())
            .ReturnsAsync(_testVotes);
        
        _voteServiceMock.Setup(x => x.GetVotesAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<VoteViewModel>());
        
        _context.Services.AddSingleton<IVoteService>(_voteServiceMock.Object);
        _context.Services.AddSingleton<IJSRuntime>(_jsRuntimeMock.Object);
        _context.Services.AddSingleton<AppConfig>(new AppConfig
        {
            MaximumFileSizeInMb = 0,
            PageSize = 0,
            ToastDurationInMillis = 0,
            HubBaseUrl = ""
        });
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public void VoteList_ShouldShowCorrectNumberOfVotes()
    {
        // Arrange
        var cut = _context.RenderComponent<VoteList>();
        
        // Assert
        var votes = cut.FindAll(".list-group-item");
        Assert.Equal(2, votes.Count);
    }

    [Fact]
    public void SelectedVote_WhenVoteClicked_ShouldShowVoteDetails()
    {
        // Arrange
        var selectedVote = _testVotes.First();

        // Act
        var cut = _context.RenderComponent<VotePage>(
            parameters => parameters
                .Add(p => p.Id, 1)
        );

        // Assert
        cut.WaitForAssertion(() =>
        {
            var voteQuestion = cut.Find("[data-testid='question']");
            Assert.Equal($"{selectedVote.Question}", voteQuestion.TextContent);

            var voteStart = cut.WaitForElements("p").First(p => p.TextContent.Contains("Start:"));
            Assert.Contains(selectedVote.Start.ToString() ?? string.Empty, voteStart.TextContent);

            var voteEnd = cut.WaitForElements("p").First(p => p.TextContent.Contains("End:"));
            Assert.Contains(selectedVote.End.ToString() ?? string.Empty, voteEnd.TextContent);
        }, timeout: TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void VoteAdd_ShouldCallServiceAndUpdateUI()
    {
        var newVote = new VoteViewModel
        {
            Id = 3,
            Question = "Will adding a new question work?",
            OptionsRaw = "Yes; No; Idk; Other; Something",
            Start = DateTime.Now + TimeSpan.FromHours(1),
            End = DateTime.Now + TimeSpan.FromHours(10),
        };

        _voteServiceMock.Setup(x => x.CreateVoteAsync(It.IsAny<VoteViewModel>()))
            .Returns(Task.CompletedTask);

        _voteServiceMock.Setup(x => x.GetVoteByIdAsync(3))
            .ReturnsAsync(newVote);

        _voteServiceMock.Setup(x => x.GetMyVotesAsync())
            .ReturnsAsync(new List<VoteViewModel>());

        _context.Services.AddSingleton(_voteServiceMock.Object);

        var cut = _context.RenderComponent<VoteAdd>();

        // Simulate user adding a vote — this part depends on your UI
        cut.Find("button[data-testid='submit-button']").Click();

        cut.Find("input[data-testid='question']").Change(newVote.Question);
        cut.Find("[data-testid='options']").Change(newVote.OptionsRaw);
        cut.Find("input[data-testid='start']").Change(newVote.Start.ToString());
        cut.Find("input[data-testid='end']").Change(newVote.End.ToString());

        cut.Find("form").Submit();

        // Assert that service was called
        cut.WaitForAssertion(() =>
        {
            _voteServiceMock.Verify(x => x.GetVotesAsync(false), Times.Once);
            _voteServiceMock.Verify(x => x.CreateVoteAsync(It.IsAny<VoteViewModel>()), Times.Once);
        });
    }
}