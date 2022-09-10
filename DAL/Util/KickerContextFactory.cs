using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Util;

public class KickerContextFactory : IDesignTimeDbContextFactory<KickerContext>
{
    public KickerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KickerContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=KickerDev;Username=postgres;Password=admin");
        return new KickerContext(optionsBuilder.Options);
    }
}