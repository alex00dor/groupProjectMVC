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
        

        public ChatHub(IRoomRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public override Task OnConnectedAsync()
        {
            User user = CurrentUser.Result;

            if (user == null)
                return base.OnConnectedAsync();

            foreach (var room in _repository.GetAllRoomsByUser(user))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            User user = CurrentUser.Result;
            if (user == null)
                return base.OnConnectedAsync();

            foreach (var room in _repository.GetAllRoomsByUser(user))
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());
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
                    .SendAsync("chat", message.User.UserName, message.Text, message.DateTime.ToString(),message.Room.Id.ToString(), message.Room.Name);
            }
        }
        
        private Task<User> CurrentUser =>
            _userManager.FindByNameAsync(Context.User.Identity.Name);
    }
}