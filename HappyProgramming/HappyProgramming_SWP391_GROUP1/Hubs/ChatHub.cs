using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using SendGrid.Helpers.Mail;

namespace SignalRChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task GetInfo()
        {
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }



        public async Task<Task> SendMessageToGroup(string sender, string receiver, string message, string groupName, string courseId)
        {
            // ChatController chat = new ChatController();
            //Pararse string to guid

            //Guid senderN = Guid.Parse(sender);
            //Guid courseIdN = Guid.Parse(courseId);


            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //await chat.SendClassMessage(senderN, courseIdN, message);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}