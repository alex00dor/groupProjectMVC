using System.Collections;
using System.Collections.Generic;
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
            if (room != null && room.RoomUsers.FirstOrDefault(u => u.RoomId == roomId && u.UserId == user.Id) != null)
            {
                return room;
            }

            return null;
        }

        public void CreateRoom(Room room, User user)
        {
            room.RoomUsers = new List<RoomUser>
            {
                new RoomUser
                {
                    Room = room,
                    User = user
                }
            };
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void LeaveRoom(int roomId, User user)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && dbEntry.RoomUsers.FirstOrDefault(u => u.RoomId == roomId && u.UserId == user.Id) !=
                null)
            {
                if (dbEntry.Private)
                {
                    DeleteRoom(roomId, user);
                }
                else
                {
                    RoomUser roomUser =
                        dbEntry.RoomUsers.FirstOrDefault(u => u.RoomId == roomId && u.UserId == user.Id);
                    dbEntry.RoomUsers.Remove(roomUser);
                    _context.SaveChanges();
                }
            }
        }
        public void DeleteRoom(int roomId, User user)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && !dbEntry.Private && dbEntry.Creator.Id == user.Id)
            {
                _context.Rooms.Remove(dbEntry);
                _context.SaveChanges();
            }else if (dbEntry != null && dbEntry.Private && dbEntry.RoomUsers.FirstOrDefault(u => u.RoomId == roomId && u.UserId == user.Id) !=
                      null)
            {
                _context.Rooms.Remove(dbEntry);
                _context.SaveChanges();
            }
        }

        public void SendMessageToRoom(int roomId, User user, Message message)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && dbEntry.RoomUsers.FirstOrDefault(u => u.RoomId == roomId && u.UserId == user.Id) !=
                null)
            {
                message.User = user;
                dbEntry.Messages = new List<Message> {message};
                _context.SaveChanges();
            }
        }

        public void AddUserToRoom(int roomId, User user)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && dbEntry.Private == false)
            {
                dbEntry.RoomUsers = new List<RoomUser>
                {
                    new RoomUser
                    {
                        Room = dbEntry,
                        User = user
                    }
                };
                _context.SaveChanges();
            }
        }

        public IQueryable<Room> GetAllRoomsByUser(User user)
        {
            return _context.Rooms.Where(r => r.RoomUsers.FirstOrDefault(ru => ru.User == user) != null);
        }

        public bool isNameUniq(string name)
        {
            Room dbEntry = _context.Rooms.FirstOrDefault(r => r.Name == name);
            return dbEntry == null;
        }
    }
}