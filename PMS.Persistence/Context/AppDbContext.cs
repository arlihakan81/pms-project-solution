using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=PMSDb; Trusted_Connection=true; TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasMany(p => p.Collaborators).WithMany(c => c.Projects).UsingEntity<Collaboration>();            
            modelBuilder.Entity<TaskItem>().HasMany(t => t.Tags).WithMany(tg => tg.Tasks).UsingEntity<TaskTag>();
        }

    }
}
