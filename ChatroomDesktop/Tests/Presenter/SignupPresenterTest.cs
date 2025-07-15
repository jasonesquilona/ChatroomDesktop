using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using ChatroomServer.Models;
using Microsoft.VisualBasic.ApplicationServices;
using Moq;
using Xunit;

namespace ChatroomDesktop.Tests.Presenter;

public class SignupPresenterTest
{
    [Fact]
    public async Task HandleConfirmClicked_WithUnusedUsername_CallsChatRoomPageList()
    {
        var mockView = new Mock<ISignupView>();
        var mockService = new Mock<INetworkService>();
        var mockMessageService = new Mock<IMessageService>();
        var chatService = new Mock<IChatService>();
        var navigatorService = new Mock<INavigatorService>();
        var userDetails = new UserDetails { UserId = 123, Username = "test" };
        var user = new UserModel {Details = userDetails, Groups = new List<GroupModel>()};
        mockView.Setup(v => v.Username).Returns("username");
        mockView.Setup(v => v.Password).Returns("password");
        mockService.Setup(s => s.SendSignupData("username","password")).ReturnsAsync(user);
        var signupPresenter = new SignupPresenter(mockView.Object, mockService.Object,navigatorService.Object,chatService.Object,mockMessageService.Object);
        mockView.Raise(v => v.EnterClicked += null, EventArgs.Empty);
        navigatorService.Verify(ns => ns.OpenChatroomListPage(chatService.Object, mockService.Object, user, navigatorService.Object, mockMessageService.Object), Times.Once());
    }

    [Fact]
    public async Task HandleConfirmClicked_WithUsedUsername_ShowsErrorMessage()
    {
        var mockView = new Mock<ISignupView>();
        var mockService = new Mock<INetworkService>();
        var mockMessageService = new Mock<IMessageService>();
        var chatService = new Mock<IChatService>();
        var navigatorService = new Mock<INavigatorService>();
        var userDetails = new UserDetails { UserId = 123, Username = "test" };
        var user = new UserModel {Details = userDetails, Groups = new List<GroupModel>()};
        mockView.Setup(v => v.Username).Returns("username");
        mockView.Setup(v => v.Password).Returns("password");
        mockService.Setup(s => s.SendSignupData("username","password")).ReturnsAsync((UserModel)null);
        
        var signupPresenter = new SignupPresenter(mockView.Object, mockService.Object,navigatorService.Object,chatService.Object,mockMessageService.Object);
        mockView.Raise(v => v.EnterClicked += null, EventArgs.Empty);
        mockMessageService.Verify(ms => ms.ShowMessage("Invalid username. Pick another one"), Times.Once); 
    }

}