using Microsoft.EntityFrameworkCore;
using HVS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HVS.Api.Core.DataAccess
{
    public class HVSNetCoreDbContext : DbContext
    {
        public HVSNetCoreDbContext(DbContextOptions<HVSNetCoreDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInRole>().HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<UserInRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserInRoles)
                .HasForeignKey(pt => pt.UserId);
            modelBuilder.Entity<UserInRole>()
                .HasOne(pt => pt.Role)
                .WithMany(p => p.UserInRoles)
                .HasForeignKey(pt => pt.RoleId);

            modelBuilder.Entity<Comment>().HasKey(t => new { t.PostId, t.UserId });
            modelBuilder.Entity<Comment>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(pt => pt.UserId);
            modelBuilder.Entity<Comment>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(pt => pt.PostId);
        }
    }
}
