using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class ProcessRequestConfiguration : IEntityTypeConfiguration<ProcessRequest>
	{
		public void Configure(EntityTypeBuilder<ProcessRequest> builder)
		{
			builder.HasKey(_ => _.Id);

			// Value Object
			builder.OwnsOne(_ => _.ApplicationNumber, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(ApplicationNumber))
					.IsRequired();

				_.HasIndex(_ => _.Value)
					.IsUnique()
					.HasDatabaseName("IX_ProcessRequests_ApplicationNumber");
			});

			builder.OwnsOne(_ => _.SerialNumber, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(SerialNumber))
					.IsRequired(false);

				_.HasIndex(_ => _.Value)
					.IsUnique(false) //Индекс неуникальный (может быть много записей с одинаковым SerialNumber).
					.HasDatabaseName("IX_ProcessRequests_SerialNumber");
			});

			builder.OwnsOne(_ => _.DescriptionMalfunction, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(DescriptionMalfunction))
					.IsRequired();
			});

			builder.OwnsOne(_ => _.InternalComment, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(InternalComment))
					.IsRequired(false);
			});

			builder.OwnsOne(_ => _.Location, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(Location))
					.IsRequired(false);
			});

			// Связи с Enumeration (справочные таблицы)
			builder.HasOne(_ => _.ApplicationType)
				.WithMany()
				.HasForeignKey(_ => _.ApplicationTypeId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(_ => _.InstrumentType)
				.WithMany()
				.HasForeignKey(_ => _.InstrumentTypeId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(_ => _.InstrumentKind)
				.WithMany()
				.HasForeignKey(_ => _.InstrumentKindId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(_ => _.InstrumentModel)
				.WithMany()
				.HasForeignKey(_ => _.InstrumentModelId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(_ => _.TypeBreakdown)
				.WithMany()
				.HasForeignKey(_ => _.TypeBreakdownId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(_ => _.ApplicationStatus)
				.WithMany()
				.HasForeignKey(_ => _.ApplicationStatusId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(_ => _.Importance)
				.WithMany()
				.HasForeignKey(_ => _.ImportanceId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);


			// Связи с WorkplaceEmployee
			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.CustomerEmployeeId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.ExecutorEmployeeId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(_ => _.CreatedAt);
			builder.HasIndex(_ => _.PlannedAt);
		}
	}
}