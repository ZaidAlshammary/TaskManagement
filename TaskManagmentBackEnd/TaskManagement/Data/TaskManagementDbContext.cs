using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using Task = TaskManagement.Models.Task;

namespace TaskManagement.Data;

public class TaskManagementDbContext : IdentityDbContext
{
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
    {
    }

    public new DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Task>()
            .HasQueryFilter(t => !t.IsDeleted);

        var hasher = new PasswordHasher<IdentityUser?>();
        modelBuilder.Entity<User>()
            .HasData(new User()
            {
                FullName = "admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = hasher.HashPassword(null, "admin"),
                IsAdmin = true
            });
    }
    
    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Entity && (e.State == EntityState.Modified || e.State == EntityState.Added));

        foreach (var entityEntry in entries)
        {
            var entity = (Entity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added && entity.CreatedAt == DateTime.MinValue)
            {
                entity.CreatedAt = DateTime.Now;
            }

            entity.UpdatedAt = DateTime.Now;
        }

        return base.SaveChanges();
    }
}