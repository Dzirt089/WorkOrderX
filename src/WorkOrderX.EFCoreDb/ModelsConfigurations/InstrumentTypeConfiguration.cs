using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class InstrumentTypeConfiguration : IEntityTypeConfiguration<InstrumentType>
	{
		public void Configure(EntityTypeBuilder<InstrumentType> builder)
		{
			builder.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.ToTable("InstrumentTypes");

			builder.HasKey(e => e.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedNever(); //Вставка новых данных с нашими ID 

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();
		}
	}
}
