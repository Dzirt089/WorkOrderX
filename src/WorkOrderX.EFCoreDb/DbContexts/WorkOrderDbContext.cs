using Microsoft.EntityFrameworkCore;

using System.Reflection;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Models.EventStores;

namespace WorkOrderX.EFCoreDb.DbContexts
{
	public class WorkOrderDbContext : DbContext
	{
		public WorkOrderDbContext(
			DbContextOptions<WorkOrderDbContext> options)
			: base(options)
		{
		}

		// Сущности
		public DbSet<WorkplaceEmployee> WorkplaceEmployees { get; set; }
		public DbSet<ProcessRequest> ProcessRequests { get; set; }

		// Модель сохранения событий (Event Store)
		public DbSet<EventStoreEntry> EventStoreEntries { get; set; }

		// Справочные данные (Enumeration)
		// Для ProcessRequest
		public DbSet<EquipmentType> EquipmentTypes { get; set; }
		public DbSet<EquipmentKind> EquipmentKinds { get; set; }
		public DbSet<EquipmentModel> EquipmentModels { get; set; }
		public DbSet<TypeBreakdown> TypeBreakdowns { get; set; }

		public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
		public DbSet<ApplicationType> ApplicationTypes { get; set; }
		public DbSet<Importance> Importances { get; set; }

		// Для Employee
		public DbSet<Role> Roles { get; set; }
		public DbSet<Specialized> Specializeds { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
