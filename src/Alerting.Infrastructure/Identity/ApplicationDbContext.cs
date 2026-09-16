using Alerting.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alerting.Infrastructure.Identity;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<AlertRule> AlertRules { get; set; } = null!;
    public DbSet<Channel> Channels { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<DeliveryLog> DeliveryLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AlertRule>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Name).IsRequired();
            e.HasMany(a => a.Subscriptions)
                .WithOne(s => s.AlertRule)
                .HasForeignKey(s => s.AlertRuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Channel>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired();
            e.Property(c => c.Type).IsRequired();
            e.Property(c => c.Address).IsRequired();
            e.HasMany(c => c.Subscriptions)
                .WithOne(s => s.Channel)
                .HasForeignKey(s => s.ChannelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Subscription>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Recipient).IsRequired();
            e.HasOne(s => s.AlertRule).WithMany(a => a.Subscriptions).HasForeignKey(s => s.AlertRuleId);
            e.HasOne(s => s.Channel).WithMany(c => c.Subscriptions).HasForeignKey(s => s.ChannelId);
        });

        builder.Entity<DeliveryLog>(e =>
        {
            e.HasKey(d => d.Id);
            e.Property(d => d.AttemptedAt).IsRequired();
            e.Property(d => d.Success).IsRequired();
            e.Property(d => d.Message).IsRequired(false);
            e.HasOne(d => d.Subscription).WithMany().HasForeignKey(d => d.SubscriptionId);
        });
    }
}
