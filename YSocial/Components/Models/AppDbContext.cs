using Microsoft.EntityFrameworkCore;
using YSocial.Components.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка таблицы users
        modelBuilder.Entity<UserAccount>()
            .ToTable("users");

        // Настройка таблицы posts
        modelBuilder.Entity<Post>()
            .ToTable("posts")
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка таблицы chat_messages
        modelBuilder.Entity<ChatMessage>()
            .ToTable("chat_messages")
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка таблицы friendships
        modelBuilder.Entity<Friendship>()
            .ToTable("friendships")
            .HasOne(f => f.User1)
            .WithMany(u => u.FriendshipsAsUser1)
            .HasForeignKey(f => f.User1Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.User2)
            .WithMany(u => u.FriendshipsAsUser2)
            .HasForeignKey(f => f.User2Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Friendship>()
            .HasIndex(f => new { f.User1Id, f.User2Id })
            .IsUnique();
    }
}