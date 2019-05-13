using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChatProject.Models.Entities
{
    public class User : IdentityUser
    {
        public List<RoomUser> RoomUsers { get; set; }
        public List<Message> Messages { get; set; }
    }
}