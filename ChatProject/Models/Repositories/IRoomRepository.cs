using System.Linq;
using ChatProject.Models.Entities;

namespace ChatProject.Models.Repositories
{
    public interface IRoomRepository
    {
        Room GetRoom(int roomId, User user);
        void CreateRoom(Room room);
        void DeleteRoom(int roomId);
        void SendMessageToRoom(int roomId, Message message);
        void AddUserToRoom(int roomId, User user);
        IQueryable<Room> GetAllRoomsByUser(User user);
    }
}