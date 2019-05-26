using System.Collections.Generic;
using System.Linq;
using ChatProject.Models.Databases;
using ChatProject.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatProject.Models.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Room> Rooms =>
            _context.Rooms.Include(x => x.RoomUsers)
                .ThenInclude(x => x.Room)
                .Include("Messages")
                .Include("Messages.User")
                .Include("Creator");

        public Room GetRoom(int roomId, User user)
        {
            Room room = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null && room.RoomUsers.FirstOrDefault(u => u.Room == room && u.User == user) != null)
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

        public void RemoveUserFromRoom(int roomId, User user)
        {
            Room dbEntry = Rooms.FirstOrDefault(r => r.Id == roomId);
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
            Room dbEntry = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && !dbEntry.Private && dbEntry.Creator == user)
            {
                _context.Rooms.Remove(dbEntry);
                _context.SaveChanges();
            }
            else if (dbEntry != null && dbEntry.Private &&
                     dbEntry.RoomUsers.FirstOrDefault(u => u.Room == dbEntry && u.User == user) !=
                     null)
            {
                _context.Rooms.Remove(dbEntry);
                _context.SaveChanges();
            }
        }

        public void SendMessageToRoom(int roomId, User user, Message message)
        {
            Room dbEntry = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null && dbEntry.RoomUsers.FirstOrDefault(u => u.Room == dbEntry && u.User == user) !=
                null)
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
            }
        }

        public void AddUserToRoom(int roomId, User user)
        {
            Room dbEntry = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry != null
                && dbEntry.Private == false
                && dbEntry.RoomUsers.FirstOrDefault(u => u.Room == dbEntry && u.User == user) == null)
            {
                dbEntry.RoomUsers.Add(
                    new RoomUser
                    {
                        Room = dbEntry,
                        User = user
                    }
                );
                _context.SaveChanges();
            }
        }

        public IQueryable<Room> GetAllRoomsByUser(User user)
        {
            return Rooms.Where(r => r.RoomUsers.FirstOrDefault(ru => ru.User == user) != null);
        }

        public bool isUserInRoom(int roomId, string userId)
        {
            Room dbEntry = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (dbEntry == null)
                return false;
            return dbEntry.RoomUsers.FirstOrDefault(u => u.RoomId == roomId && u.UserId == userId) != null;
        }

        public bool isNameUniq(string name)
        {
            Room dbEntry = Rooms.FirstOrDefault(r => r.Name == name);
            return dbEntry == null;
        }
    }
}