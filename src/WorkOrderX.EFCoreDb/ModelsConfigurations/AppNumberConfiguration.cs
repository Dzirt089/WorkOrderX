using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.EFCoreDb.Models;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class AppNumberConfiguration : IEntityTypeConfiguration<AppNumber>
	{
		public void Configure(EntityTypeBuilder<AppNumber> builder)
		{
			builder.ToTable("Numbers");
			builder.HasKey(_ => _.Id);

			builder.Property(_ => _.Id)
				.ValueGeneratedNever();

			builder.Property(_ => _.Number)
				.IsRequired();
		}
	}
}
