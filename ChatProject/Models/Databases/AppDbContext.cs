using ChatProject.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ChatProject.Models.Databases
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoomUser>()
                .HasKey(t => new {t.UserId, t.RoomId});

            builder.Entity<RoomUser>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.RoomUsers)
                .HasForeignKey(pt => pt.UserId);

            builder.Entity<RoomUser>()
                .HasOne(pt => pt.Room)
                .WithMany(t => t.RoomUsers)
                .HasForeignKey(pt => pt.RoomId);

            builder.Entity<Message>().HasKey(t => new {t.UserId, t.RoomId});

            builder.Entity<Message>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Messages)
                .HasForeignKey(pt => pt.UserId);
            
            builder.Entity<Message>()
                .HasOne(pt => pt.Room)
                .WithMany(p => p.Messages)
                .HasForeignKey(pt => pt.RoomId);
        }


        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}