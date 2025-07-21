using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class ImportanceConfiguration : IEntityTypeConfiguration<Importance>
	{
		public void Configure(EntityTypeBuilder<Importance> builder)
		{
			builder.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.ToTable("Importances");

			builder.HasKey(i => i.Id);

			builder.Property(_ => _.Id)
				.ValueGeneratedNever(); //Вставка новых данных с нашими ID 

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();
		}
	}
}
