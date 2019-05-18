using System;
using ChatProject.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

//            builder.Entity<RoomUser>()
//                .HasKey(t => new {t.UserId, t.RoomId});

            builder.Entity<RoomUser>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.RoomUsers)
                .HasForeignKey(pt => pt.UserId);

            builder.Entity<RoomUser>()
                .HasOne(pt => pt.Room)
                .WithMany(t => t.RoomUsers)
                .HasForeignKey(pt => pt.RoomId);

            //builder.Entity<Message>().HasKey(t => new {t.UserId, t.RoomId});

            builder.Entity<Message>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Messages)
                .HasForeignKey(pt => pt.UserId);
            
            builder.Entity<Message>()
                .HasOne(pt => pt.Room)
                .WithMany(p => p.Messages)
                .HasForeignKey(pt => pt.RoomId);
            
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(dateTimeConverter);
                }
            }
        }


        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}