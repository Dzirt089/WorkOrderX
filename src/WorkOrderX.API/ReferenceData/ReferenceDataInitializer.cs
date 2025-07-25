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
				await SeedReferenceDataAsync(dbContext, cancellationToken);
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
			if (!await dbContext.EquipmentTypes.AnyAsync(cancellationToken))
				await AddEnumAsync<EquipmentType>(dbContext, cancellationToken);

			if (!await dbContext.EquipmentKinds.AnyAsync(cancellationToken))
				await AddEnumAsync<EquipmentKind>(dbContext, cancellationToken);

			if (!await dbContext.TypeBreakdowns.AnyAsync(cancellationToken))
				await AddEnumAsync<TypeBreakdown>(dbContext, cancellationToken);

			if (!await dbContext.ApplicationStatuses.AnyAsync(cancellationToken))
				await AddEnumAsync<ApplicationStatus>(dbContext, cancellationToken);

			if (!await dbContext.ApplicationTypes.AnyAsync(cancellationToken))
				await AddEnumAsync<ApplicationType>(dbContext, cancellationToken);

			if (!await dbContext.Roles.AnyAsync(cancellationToken))
				await AddEnumAsync<Role>(dbContext, cancellationToken);

			if (!await dbContext.Specializeds.AnyAsync(cancellationToken))
				await AddEnumAsync<Specialized>(dbContext, cancellationToken);

			if (!await dbContext.EquipmentModels.AnyAsync(cancellationToken))
				await AddEnumAsync<EquipmentModel>(dbContext, cancellationToken);

			if (!await dbContext.Importances.AnyAsync(cancellationToken))
				await AddEnumAsync<Importance>(dbContext, cancellationToken);
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
			await SyncEnumAsync<Importance>(dbContext, cancellationToken);
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
			await AddMissingItemsAsync<T>(db, cancellationToken);
			await UpdateOrRemoveItemsAsync<T>(db, cancellationToken);
		}

		/// <summary>
		/// Обновление или удаление элементов перечисления в базе данных.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="db"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private static async Task UpdateOrRemoveItemsAsync<T>(DbContext db, CancellationToken cancellationToken) where T : Enumeration
		{
			// Получаем все элементы из базы данных и перечисления
			var dbItems = await db.Set<T>().ToListAsync(cancellationToken);

			// Получаем все элементы перечисления
			var codeItems = Enumeration.GetAll<T>().ToList();

			// Находим элементы, которые есть в базе данных, но отсутствуют в перечислении
			var extraItems = dbItems.Where(dbItem => !codeItems.Any(codeItem => codeItem.Id == dbItem.Id)).ToList();

			// Если есть лишние элементы, удаляем их из базы данных
			if (extraItems.Any())
			{
				db.Set<T>().RemoveRange(extraItems);

				// Удаляем лишние элементы и из списка. Чтобы не было неожиданных исключений
				foreach (var item in extraItems)
				{
					dbItems.Remove(item);
				}
			}



			dbItems.ForEach(_ =>
			{
				// Находим соответствующий элемент в перечислении по Id
				var codeItem = Enumeration.FromId<T>(_.Id);

				if (codeItem != null)
				{
					// Проверяем, нужно ли обновлять значения
					bool upNameDescr = _.Name != codeItem.Name || _.Descriptions != codeItem.Descriptions;

					// Если это EquipmentKind или TypeBreakdown, проверяем дополнительные поля
					if (_ is TypeBreakdown dbBreakdown && codeItem is TypeBreakdown codeBreakdown)
					{
						upNameDescr = upNameDescr || dbBreakdown.EquipmentTypeId != codeBreakdown.EquipmentTypeId;
					}

					if (_ is EquipmentKind dbKind && codeItem is EquipmentKind codeKind)
					{
						upNameDescr = upNameDescr || dbKind.EquipmentTypeId != codeKind.EquipmentTypeId;
					}

					// Если нужно обновить, устанавливаем новые значения
					if (upNameDescr)
					{
						// Обновляем значения в базе данных
						db.Entry(_).CurrentValues.SetValues(codeItem);
					}
				}
			});


		}

		/// <summary>
		/// Добавление отсутствующих элементов перечисления в базу данных.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="db"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task AddMissingItemsAsync<T>(DbContext db, CancellationToken cancellationToken) where T : Enumeration
		{
			// Получаем все элементы из базы данных и перечисления
			var dbItems = await db.Set<T>().ToListAsync(cancellationToken);

			// Получаем все элементы перечисления
			var codeItems = Enumeration.GetAll<T>().ToList();

			// Находим элементы, которые есть в перечислении, но отсутствуют в базе данных
			var missing = codeItems.Where(codeItem => !dbItems.Any(dbItem => dbItem.Id == codeItem.Id)).ToList();

			// Если есть отсутствующие элементы, добавляем их в базу данных
			if (missing.Any())
			{
				// Если это EquipmentKind или TypeBreakdown, устанавливаем EquipmentType из ChangeTracker этим элементам.
				// Так как только у этих двух перечислений есть EquipmentTypeId, то проверяем только их
				if (typeof(T) == typeof(EquipmentKind) || typeof(T) == typeof(TypeBreakdown))
				{
					// Получаем все EquipmentType из ChangeTracker базы данных
					var trackedTypeDict = db.Set<EquipmentType>().Local.ToDictionary(_ => _.Id);

					// Проходим по отсутствующим элементам и устанавливаем EquipmentType, если он есть в ChangeTracker
					missing.ForEach(_ =>
					{
						switch (_)
						{
							// Если это EquipmentKind, устанавливаем EquipmentType взятый из ChangeTracker элементу, который добавляем в базу данных
							case EquipmentKind kind:
								if (trackedTypeDict.TryGetValue(kind.EquipmentTypeId, out var type))
									kind.SetEquipmentType(type);
								break;

							// Если это TypeBreakdown, устанавливаем EquipmentType взятый из ChangeTracker элементу, который добавляем в базу данных
							case TypeBreakdown breakdown:
								if (trackedTypeDict.TryGetValue(breakdown.EquipmentTypeId, out var type2))
									breakdown.SetEquipmentType(type2);
								break;
						}
					});
				}

				// Добавляем отсутствующие элементы в базу данных
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
