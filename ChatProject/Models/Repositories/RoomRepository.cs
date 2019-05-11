using System.Linq;
using ChatProject.Models.Databases;
using ChatProject.Models.Entities;

namespace ChatProject.Models.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public Room GetRoom(int roomId, User user)
        {
            Room room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null && room.Users.Contains(user))
            {
                return room;
            }
            
            return null;
        }

        public void CreateRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void DeleteRoom(int roomId)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null)
            {
                _context.Rooms.Remove(dbEntry);
                _context.SaveChanges();
            }
        }

        public void SendMessageToRoom(int roomId, Message message)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null)
            {
                dbEntry.Messages.Add(message);
                _context.SaveChanges();
            }
        }

        public void AddUserToRoom(int roomId, User user)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && dbEntry.Private == false)
            {
                dbEntry.Users.Add(user);
                _context.SaveChanges();
            }
        }

        public IQueryable<Room> GetAllRoomsByUser(User user)
        {
            return _context.Rooms.Where(r => r.Users.Contains(user));
        }
    }
}