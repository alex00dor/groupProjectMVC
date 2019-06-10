using System.Collections.Generic;
using ChatProject.Models.Entities;

namespace ChatProject.Models.ViewModels
{
    public class RoomsListModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        
        public string SearchRequest { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}