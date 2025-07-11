using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class EquipmentModelConfiguration : IEntityTypeConfiguration<EquipmentModel>
	{
		public void Configure(EntityTypeBuilder<EquipmentModel> builder)
		{
			builder.ToTable("EquipmentModels");

			builder.HasKey(e => e.Id);

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();
		}
	}
}
