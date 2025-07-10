using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
	{
		public void Configure(EntityTypeBuilder<EquipmentType> builder)
		{
			builder.ToTable("EquipmentTypes");

			builder.HasKey(e => e.Id);

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();
		}
	}
}
