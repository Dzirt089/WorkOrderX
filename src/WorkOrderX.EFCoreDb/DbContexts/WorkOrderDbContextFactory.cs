using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WorkOrderX.EFCoreDb.DbContexts
{
	public class WorkOrderDbContextFactory : IDesignTimeDbContextFactory<WorkOrderDbContext>
	{
		public WorkOrderDbContext CreateDbContext(string[] args)
		{
			var connectionString = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.AddEnvironmentVariables()
			.Build();


			var optionsBuilder = new DbContextOptionsBuilder<WorkOrderDbContext>();
			optionsBuilder.UseSqlServer(connectionString.GetConnectionString("ConnectionString"),
				opt => opt.UseCompatibilityLevel(110));

			return new WorkOrderDbContext(optionsBuilder.Options);
		}
	}
}
