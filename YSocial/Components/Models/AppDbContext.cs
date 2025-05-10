namespace YSocial.Components;

using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Post> Posts { get; set; }
}