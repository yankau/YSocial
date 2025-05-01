using Microsoft.EntityFrameworkCore;

namespace YSocial.Components.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<UserAccount> UserAccounts { get; set; }
}