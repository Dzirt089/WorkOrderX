using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class ApplicationTypeConfiguration : IEntityTypeConfiguration<ApplicationType>
	{
		public void Configure(EntityTypeBuilder<ApplicationType> builder)
		{
			builder.ToTable("ApplicationTypes");

			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedNever();

			builder.Property(_ => _.Name)
				.IsRequired();

			builder.Property(_ => _.Descriptions)
				.IsRequired();
		}
	}
}
