using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VotingSystem.Blazor.WebAssembly.Layout;
using VotingSystem.Blazor.WebAssembly.Services;

namespace VotingSystem.Blazor.ComponentTests;

public class MenuComponentTests : IDisposable
{
    private readonly TestContext _context = new();
    private readonly Mock<IAuthenticationService> _authenticationServiceMock = new();
    private readonly FakeNavigationManager _fakeNavigationManager;

    public MenuComponentTests()
    {
        // Setup mocks and register services
        _authenticationServiceMock
            .Setup(x => x.GetCurrentlyLoggedInUserAsync())
            .ReturnsAsync("user2");
        
        _context.Services.AddSingleton<IAuthenticationService>(_authenticationServiceMock.Object);
        _fakeNavigationManager = _context.Services.GetRequiredService<FakeNavigationManager>();
    }

    public void Dispose() => _context.Dispose();
    
    [Fact]
    public void Navbar_WhenUserIsAuthenticated_ShouldShowFullMenu()
    {
        var cut = _context.RenderComponent<MenuComponent>();
        
        var brand = cut.Find(".app-title");
        Assert.Equal("Anonymous Voting Administration", brand.TextContent);

        var menu = cut.FindAll(".nav-item-link");
        Assert.NotEmpty(menu);
        
        var welcomeText = cut.Find("[data-testid='welcome']");
        Assert.Equal("Welcome, user2!", welcomeText.TextContent);
        
        var logoutButton = cut.Find("[data-testid='logout']");
        Assert.Equal("Logout", logoutButton.TextContent.Trim());
    }
    
    [Fact]
    public void LogoutButton_WhenClicked_ShouldCallLogoutAndRedirectToLoginPage()
    {
        // Arrange
        var cut = _context.RenderComponent<MenuComponent>();
        
        // Act
        var logoutButton = cut.Find("[data-testid='logout']");
        
        // Assert
        logoutButton.Click();
        // https://bunit.dev/docs/verification/async-assertion.html
        cut.WaitForAssertion(() =>
        {
            Assert.Equal("http://localhost/login", _fakeNavigationManager.Uri);
            _authenticationServiceMock.Verify(x => x.LogoutAsync(), Times.Once);
        });
    }
}