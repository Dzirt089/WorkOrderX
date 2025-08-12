using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	internal class TypeBreakdownConfiguration : IEntityTypeConfiguration<TypeBreakdown>
	{
		public void Configure(EntityTypeBuilder<TypeBreakdown> builder)
		{
			builder.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.ToTable("TypeBreakdowns");

			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedNever(); //Вставка новых данных с нашими ID 

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();

			builder.HasOne(_ => _.InstrumentType)
				.WithMany()
				.HasForeignKey(_ => _.InstrumentTypeId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
