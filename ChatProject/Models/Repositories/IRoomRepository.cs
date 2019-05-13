using System.Linq;
using ChatProject.Models.Entities;

namespace ChatProject.Models.Repositories
{
    public interface IRoomRepository
    {
        Room GetRoom(int roomId, User user);
        void CreateRoom(Room room, User user);
        void DeleteRoom(int roomId, User user);
        void LeaveRoom(int roomId, User user);
        void SendMessageToRoom(int roomId, User user, Message message);
        void AddUserToRoom(int roomId, User user);
        IQueryable<Room> GetAllRoomsByUser(User user);

        bool isNameUniq(string name);
    }
}