using Accessibility;
using ChatroomDesktop.Models;
using ChatroomDesktop.Models.EventArgs;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using ChatroomServer.Models;
using Moq;
using Xunit;

namespace ChatroomDesktop.Tests.Presenter;

public class GroupChatListPresenterTest
{
    [Fact]
    private async Task HandleGroupButtonClicked_CallsChatroomPage()
    {
        var mockView = new Mock<IGroupChatsView>();
        var mockService = new Mock<INetworkService>();
        var mockMessageService = new Mock<IMessageService>();
        var navigatorService = new Mock<INavigatorService>();
        var chatService = new Mock<IChatService>();
        var userDetails = new UserDetails { UserId = 123, Username = "test" };
        
        var user = new UserModel {Details = userDetails, Groups = {new GroupModel{GroupName = "test", GroupId = "1A2B3"} }};
        
        var groupChatListPresenter = new GroupChatListPresenter(mockView.Object, mockService.Object, chatService.Object, user,navigatorService.Object, mockMessageService.Object);

        var eventArgs = new GroupButtonEventArgs { GroupId = "1A2B3", GroupName = "test" };

        //mockView.Raise(v => v.GroupButtonClicked += null, new GroupButtonEventArgs{GroupId = "1A2B3", GroupName = "test"});
    }
}