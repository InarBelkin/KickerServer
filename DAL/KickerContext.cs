using DAL.Entities;
using DAL.Entities.Auth;
using DAL.Entities.Battle;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class KickerContext : DbContext
{
    //Null-safety безумие
#pragma warning disable CS8618
    public KickerContext(DbContextOptions<KickerContext> options) : base(options)
#pragma warning restore CS8618
    {
        //Database.EnsureCreated();
    }

    public DbSet<AuthInfo> AuthInfos { get; set; }
    public DbSet<AuthInfoFirebase> AuthInfosFirebase { get; set; }
    public DbSet<AuthInfoMail> AuthInfosMail { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<StatsOneVsOne> StatsOneVsOnes { get; set; }
    public DbSet<StatsTwoVsTwo> StatsTwoVsTwos { get; set; }

    public DbSet<Battle> Battles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Battle>()
            .HasMany(c => c.Users)
            .WithMany(s => s.Battles)
            .UsingEntity<UserBattle>(
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserBattles)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Battle)
                    .WithMany(p => p.UserBattles)
                    .HasForeignKey(pt => pt.BattleId),
                j =>
                {
                    j.HasKey(t => new {t.BattleId, t.UserId});
                    j.ToTable("UserBattles");
                });
        modelBuilder.Entity<StatsOneVsOne>().Property(s => s.ELO).HasDefaultValue(1000);
    }
}