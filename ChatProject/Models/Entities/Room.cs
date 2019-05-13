using System;
using System.Collections.Generic;

namespace ChatProject.Models.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual User Creator { get; set; }
        public bool Private { get; set; }
        public virtual List<RoomUser> RoomUsers { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}