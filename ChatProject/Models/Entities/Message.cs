using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatProject.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public string Text { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateTime { get; set; }
    }
}