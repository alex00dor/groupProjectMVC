using System;

namespace ChatProject.Models.Entities
{
    public class Message
    {
        public int ID { get; set; }
        public virtual User User { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }
}