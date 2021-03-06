using System.Collections.Generic;
using ChatProject.Models.Entities;

namespace ChatProject.Models.ViewModels
{
    public class RoomModel
    {
        public List<User> Users { get; set; }
        public Room Room { get; set; }
        
        public IEnumerable<Room> UserRooms { get; set; }
        public string CurrentUserId { get; set; }
    }
}