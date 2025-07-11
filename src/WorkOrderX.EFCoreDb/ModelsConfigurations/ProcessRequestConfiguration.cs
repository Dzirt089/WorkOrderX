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
					.IsUnique()
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

			// Связи с Enumeration (справочные таблицы)
			builder.HasOne(_ => _.ApplicationType)
				.WithMany()
				.HasForeignKey(_ => _.ApplicationTypeId)
				.IsRequired();

			builder.HasOne(_ => _.EquipmentType)
				.WithMany()
				.HasForeignKey(_ => _.EquipmentTypeId)
				.IsRequired(false);

			builder.HasOne(_ => _.EquipmentKind)
				.WithMany()
				.HasForeignKey(_ => _.EquipmentKindId)
				.IsRequired(false);

			builder.HasOne(_ => _.EquipmentModel)
				.WithMany()
				.HasForeignKey(_ => _.EquipmentModelId)
				.IsRequired(false);

			builder.HasOne(_ => _.TypeBreakdown)
				.WithMany()
				.HasForeignKey(_ => _.TypeBreakdownId)
				.IsRequired();

			builder.HasOne(_ => _.ApplicationStatus)
				.WithMany()
				.HasForeignKey(_ => _.ApplicationStatusId)
				.IsRequired();


			// Связи с WorkplaceEmployee
			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.CustomerEmployeeId)
				.IsRequired();

			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.ExecutorEmployeeId)
				.IsRequired();
		}
	}
}