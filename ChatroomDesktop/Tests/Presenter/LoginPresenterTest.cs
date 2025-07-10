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
        var mockMessageService = new Mock<IMessageService>();
        var navigatorService = new Mock<INavigatorService>();
        var chatService = new Mock<IChatService>();
        var user = new UserModel {Username = "test", UserId = 123, Groups = new List<GroupModel>()};
        mockView.Setup(v => v.Name).Returns("test");
        mockView.Setup(v => v.Password).Returns("password");
        mockService.Setup(s => s.CheckCredentials("test", "password")).ReturnsAsync(user);
        mockService.Setup(s => s.SetUpConnection()).Returns(Task.CompletedTask);
        
        var presenter = new LoginPresenter(mockView.Object, mockService.Object, mockMessageService.Object, navigatorService.Object, chatService.Object);
        
        //Trigger EnterClicked Event
        mockView.Raise(v => v.EnterClicked += null, EventArgs.Empty);
        
        // Assert
        mockService.Verify(s => s.CheckCredentials("test", "password"), Times.Once);
        mockService.Verify(s => s.SetUpConnection(), Times.Once);
        mockView.Verify(v => v.IncorrectLoginDetails(), Times.Never);
    }

    [Fact]
    public async Task HandleEnterClicked_WithInvalidCrentials_CallsFailedLogin()
    {
        var mockView = new Mock<ILoginView>();
        var mockService = new Mock<INetworkService>();
        var mockMessageService = new Mock<IMessageService>();
        var user = new UserModel();
        var chatService = new Mock<IChatService>();
        var navigatorService = new Mock<INavigatorService>();
        mockView.Setup(v => v.Name).Returns("test");
        mockView.Setup(v => v.Password).Returns("password");
        mockService.Setup(s => s.CheckCredentials("test", "password")).ReturnsAsync((UserModel)null);
        
        var presenter = new LoginPresenter(mockView.Object, mockService.Object, mockMessageService.Object, navigatorService.Object, (chatService.Object));
        
        mockView.Raise(v => v.EnterClicked += null, EventArgs.Empty);
        mockService.Verify(s => s.CheckCredentials("test", "password"), Times.Once);
        mockView.Verify(v => v.IncorrectLoginDetails(), Times.Once);
        mockMessageService.Verify(ms => ms.ShowMessage("Wrong username or password!"), Times.Once);
    }

    [Fact]
    public async Task HandleSignupClicked()
    {
        var mockView = new Mock<ILoginView>();
        var mockService = new Mock<INetworkService>();
        var mockMessageService = new Mock<IMessageService>();
        var user = new UserModel();
        var chatService = new Mock<IChatService>();
        var navigatorService = new Mock<INavigatorService>();

        var presenter = new LoginPresenter(mockView.Object,mockService.Object, mockMessageService.Object, navigatorService.Object, chatService.Object);
        
        mockView.Raise(v => v.SignUpClicked += null, EventArgs.Empty);
        navigatorService.Verify(ns => ns.OpenSignupPage(mockService.Object, chatService.Object, navigatorService.Object, mockMessageService.Object), Times.Once);

    }
}