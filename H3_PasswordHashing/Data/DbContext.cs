using H3_PasswordHashing.Models;

namespace H3_PasswordHashing.Data;
using Microsoft.EntityFrameworkCore;

public class PasswordHashingDbContext : DbContext
{
    public PasswordHashingDbContext(DbContextOptions<PasswordHashingDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=:memory:");
        }
    }

    public DbSet<User> Users { get; set; }
}