using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class InstrumentKindConfiguration : IEntityTypeConfiguration<InstrumentKind>
	{
		public void Configure(EntityTypeBuilder<InstrumentKind> builder)
		{
			builder.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.ToTable("InstrumentKinds");

			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedNever();

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();

			builder.HasOne(_ => _.InstrumentType) // Навигационное св-во
				.WithMany() // Один тип -> много видов
				.HasForeignKey(_ => _.InstrumentTypeId) // Внешний ключ
				.IsRequired() // Обязательный
				.OnDelete(DeleteBehavior.Restrict); // Запрет на каскадное удаление			
		}
	}
}
