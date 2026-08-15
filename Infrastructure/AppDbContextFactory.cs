using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace Infrastructure
{
    //This class is as the DbContext is not in the startup project(04.Web), so EF cannot automatically find the appsettings.json file
    //or have DI container to access any configuration.This way gives EF a guaranteed way to instantiate your DbContext without the 04.Web project.
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            //Need to do this to get the Connection string from the appsettings.json file in the Web project, since this factory is in the Infrastructure project.
            //I've set the connection string on user secrets in the Web project, so I need to read the UserSecretsId from the Web project file and use that to load the user secrets.
            var webProjectFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "04-Web");
            var webProjectFile = Path.Combine(webProjectFolder, "04.Web.csproj");

            var csprojXml = XDocument.Load(webProjectFile);

            var userSecretsId = csprojXml
                .Descendants("UserSecretsId")
                .FirstOrDefault()?.Value;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(webProjectFolder)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true);

            if (!string.IsNullOrEmpty(userSecretsId))
            {
                configBuilder.AddUserSecrets(userSecretsId);
            }

            var config = configBuilder.Build();

            var connectionString = config.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
