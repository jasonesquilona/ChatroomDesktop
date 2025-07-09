using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Moq;

namespace ChatroomDesktop.Tests.Presenter;
using Xunit;

public class LoginPresenterTest
{
    [Fact]
    public async Task HandleEnterClicked_WithValidCrentials_CallsSuccessfulLogin()
    {
        var mockView = new Mock<ILoginView>();
        var mockService = new Mock<INetworkService>();
        var user = new UserModel {Username = "test", UserId = 123, Groups = new List<GroupModel>()};
        mockView.Setup(v => v.Name).Returns("test");
        mockView.Setup(v => v.Password).Returns("password");
        mockService.Setup(s => s.CheckCredentials("test", "password")).ReturnsAsync(user);
        mockService.Setup(s => s.SetUpConnection("test")).Returns(Task.CompletedTask);
        
        var presenter = new LoginPresenter(mockView.Object, user, mockService.Object);
        
        //Trigger EnterClicked Event
        mockView.Raise(v => v.EnterClicked += null, EventArgs.Empty);
        
        // Assert
        mockService.Verify(s => s.CheckCredentials("test", "password"), Times.Once);
        mockService.Verify(s => s.SetUpConnection("test"), Times.Once);
        mockView.Verify(v => v.IncorrectLoginDetails(), Times.Never);
    }
}