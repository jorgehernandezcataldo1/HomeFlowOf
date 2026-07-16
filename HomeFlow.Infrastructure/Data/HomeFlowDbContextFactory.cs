using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HomeFlow.Infrastructure.Data
{
    /// <summary>
    /// Factory para DbContext en tiempo de diseño
    /// Utilizado por Entity Framework Core para crear migraciones
    /// </summary>
    public class HomeFlowDbContextFactory : IDesignTimeDbContextFactory<HomeFlowDbContext>
    {
        public HomeFlowDbContext CreateDbContext(string[] args)
        {
            // Construir la configuración desde appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "HomeFlow.Web"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<HomeFlowDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly("HomeFlow.Infrastructure");
                sqlOptions.CommandTimeout(30);
            });

            return new HomeFlowDbContext(optionsBuilder.Options);
        }
    }
}
