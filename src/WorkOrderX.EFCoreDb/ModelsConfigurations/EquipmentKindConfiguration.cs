using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class EquipmentKindConfiguration : IEntityTypeConfiguration<EquipmentKind>
	{
		public void Configure(EntityTypeBuilder<EquipmentKind> builder)
		{
			builder.ToTable("EquipmentKinds");

			builder.HasKey(e => e.Id);

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();

			builder.HasOne<EquipmentType>() // Навигационное св-во
				.WithMany() // Один тип -> много видов
				.HasForeignKey(_ => _.EquipmentTypeId) // Внешний ключ
				.IsRequired() // Обязательный
				.OnDelete(DeleteBehavior.Restrict); // Запрет на каскадное удаление			
		}
	}
}
