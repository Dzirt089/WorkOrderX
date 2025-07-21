using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.ToTable("Roles");

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
