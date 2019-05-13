using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatProject.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public string Text { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateTime { get; set; }
    }
}