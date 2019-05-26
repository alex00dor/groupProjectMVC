using System;
using System.Collections;
using System.Threading.Tasks;
using ChatProject.Models.Entities;
using ChatProject.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatProject.Hubs
{
    public class ChatHub : Hub
    {
        private IRoomRepository _repository;
        private UserManager<User> _userManager;

        private Hashtable userGroups = new Hashtable();

        public ChatHub(IRoomRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        
        public async Task JoinGroup(int roomId)
        {
            User user = await CurrentUser;
            if (_repository.isUserInRoom(roomId, user.Id))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                userGroups.Add(Context.ConnectionId, roomId);
            }
        }

        public async Task LeaveGroup(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (userGroups.ContainsKey(Context.ConnectionId))
            {
                LeaveGroup((int) userGroups[Context.ConnectionId]);
                userGroups.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        [Authorize]
        public async Task SendMessage(int roomId, string text)
        {
            User user = await CurrentUser;
            if (user != null && text != null && text.Trim() != "" && _repository.isUserInRoom(roomId, user.Id))
            {
                Message message = new Message
                {
                    User = user,
                    Room = _repository.GetRoom(roomId, user),
                    Text = text.Trim(),
                    DateTime = DateTime.Now
                };

                _repository.SendMessageToRoom(roomId, user, message);
                await Clients.Groups(roomId.ToString())
                    .SendAsync("chat", message.User.UserName, message.Text, message.DateTime.ToString(), user.Id);
            }
        }
        
        private Task<User> CurrentUser =>
            _userManager.FindByNameAsync(Context.User.Identity.Name);
    }
}