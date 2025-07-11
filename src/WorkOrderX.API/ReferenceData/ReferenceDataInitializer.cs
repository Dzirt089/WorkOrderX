
using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.API.ReferenceData
{
	public class ReferenceDataInitializer : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<ReferenceDataInitializer> _logger;

		public ReferenceDataInitializer(IServiceProvider serviceProvider, ILogger<ReferenceDataInitializer> logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		/// <summary>
		/// Остановка инициализации базы данных.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

		/// <summary>
		/// Запуск инициализации базы данных.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting database initialization");

			using var scope = _serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<WorkOrderDbContext>();

			await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
			try
			{
				// Проверяем одну таблицу на наличие данных. Если она пустая, то заполняем все справочные данные. Т.к. заполнение данных идёт сразу для 3-х таблиц.
				if (!await dbContext.EquipmentTypes.AnyAsync(cancellationToken))
				{
					await SeedReferenceDataAsync(dbContext, cancellationToken);
				}

				await SynchronizeAsync(dbContext, cancellationToken);

				await dbContext.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);

				_logger.LogError(ex, "Database initialization failed");
				throw;
			}

			_logger.LogInformation("Database initialized successfully");
		}

		/// <summary>
		/// Заполнение справочных данных в базу данных.
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task SeedReferenceDataAsync(WorkOrderDbContext dbContext, CancellationToken cancellationToken)
		{
			await AddEnumAsync<EquipmentType>(dbContext, cancellationToken);
			await AddEnumAsync<EquipmentKind>(dbContext, cancellationToken);
			await AddEnumAsync<TypeBreakdown>(dbContext, cancellationToken);
			await AddEnumAsync<ApplicationStatus>(dbContext, cancellationToken);
			await AddEnumAsync<ApplicationType>(dbContext, cancellationToken);
			await AddEnumAsync<Role>(dbContext, cancellationToken);
			await AddEnumAsync<Specialized>(dbContext, cancellationToken);
			await AddEnumAsync<EquipmentModel>(dbContext, cancellationToken);
		}

		/// <summary>
		/// Синхронизация справочных данных с базой данных.
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task SynchronizeAsync(WorkOrderDbContext dbContext, CancellationToken cancellationToken)
		{

			await SyncEnumAsync<EquipmentType>(dbContext, cancellationToken);
			await SyncEnumAsync<EquipmentKind>(dbContext, cancellationToken);
			await SyncEnumAsync<TypeBreakdown>(dbContext, cancellationToken);
			await SyncEnumAsync<ApplicationStatus>(dbContext, cancellationToken);
			await SyncEnumAsync<ApplicationType>(dbContext, cancellationToken);
			await SyncEnumAsync<Role>(dbContext, cancellationToken);
			await SyncEnumAsync<Specialized>(dbContext, cancellationToken);
			await SyncEnumAsync<EquipmentModel>(dbContext, cancellationToken);
		}

		/// <summary>
		/// Синхронизация перечисления с базой данных.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="db"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task SyncEnumAsync<T>(DbContext db, CancellationToken cancellationToken) where T : Enumeration
		{

			var equipmentTypesBd = await db.Set<T>().Select(_ => _.Id).ToListAsync(cancellationToken);
			var missing = Enumeration.GetAll<T>().Where(_ => !equipmentTypesBd.Contains(_.Id)).ToList();

			if (missing.Any())
			{
				await db.Set<T>().AddRangeAsync(missing, cancellationToken);
			}
		}

		/// <summary>
		/// Добавление перечисления в базу данных, если его там нет.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="db"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task AddEnumAsync<T>(DbContext db, CancellationToken cancellationToken) where T : Enumeration
		{
			var equipmentTypes = Enumeration.GetAll<T>().ToList();
			await db.Set<T>().AddRangeAsync(equipmentTypes, cancellationToken);
		}
	}
}
