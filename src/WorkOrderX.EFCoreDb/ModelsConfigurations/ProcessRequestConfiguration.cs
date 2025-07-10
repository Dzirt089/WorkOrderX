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
			});

			builder.OwnsOne(_ => _.SerialNumber, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(SerialNumber))
					.IsRequired(false);
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
			builder.HasOne<ApplicationType>()
				.WithMany()
				.HasForeignKey("ApplicationTypeId")
				.IsRequired();

			builder.HasOne<EquipmentType>()
				.WithMany()
				.HasForeignKey("EquipmentTypeId")
				.IsRequired(false);

			builder.HasOne<EquipmentKind>()
				.WithMany()
				.HasForeignKey("EquipmentKindId")
				.IsRequired(false);

			builder.HasOne<EquipmentModel>()
				.WithMany()
				.HasForeignKey("EquipmentModelId")
				.IsRequired(false);

			builder.HasOne<TypeBreakdown>()
				.WithMany()
				.HasForeignKey("TypeBreakdownId")
				.IsRequired();

			builder.HasOne<ApplicationStatus>()
				.WithMany()
				.HasForeignKey("ApplicationStatusId")
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