using System.Collections.Generic;
using ChatProject.Models.Entities;

namespace ChatProject.Models.ViewModels
{
    public class RoomsListModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}