using System.Linq;
using ChatProject.Models.Entities;

namespace ChatProject.Models.Repositories
{
    public interface IRoomRepository
    {
        IQueryable<Room> Rooms { get; }
        Room GetRoom(int roomId, User user);
        void CreateRoom(Room room, User user);
        void DeleteRoom(int roomId, User user);
        void RemoveUserFromRoom(int roomId, User user);
        void SendMessageToRoom(int roomId, User user, Message message);
        void AddUserToRoom(int roomId, User user);
        IQueryable<Room> GetAllRoomsByUser(User user);

        bool isNameUniq(string name);
    }
}