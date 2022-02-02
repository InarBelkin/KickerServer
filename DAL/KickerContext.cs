using DAL.Entities;
using DAL.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class KickerContext : DbContext
{
    //Null-safety безумие
#pragma warning disable CS8618
    public KickerContext(DbContextOptions<KickerContext> options) : base(options)
#pragma warning restore CS8618
    {
    }

    private DbSet<AuthInfo> AuthInfos { get; set; }
    private DbSet<AuthInfoFirebase> AuthInfosFirebase { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<StatsOneVsOne> StatsOneVsOnes { get; set; }
    public DbSet<StatsTwoVsTwo> StatsTwoVsTwos { get; set; }
}