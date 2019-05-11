using System;
using System.Collections.Generic;

namespace ChatProject.Models.Entities
{
    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Boolean Private { get; set; }
        
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}