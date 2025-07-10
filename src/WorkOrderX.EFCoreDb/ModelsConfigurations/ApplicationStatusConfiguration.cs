using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class ApplicationStatusConfiguration : IEntityTypeConfiguration<ApplicationStatus>
	{
		public void Configure(EntityTypeBuilder<ApplicationStatus> builder)
		{
			builder.ToTable("ApplicationStatuses");

			builder.HasKey(e => e.Id);
			builder.Property(_ => _.Name)
				.IsRequired();
			builder.Property(_ => _.Descriptions)
				.IsRequired();
		}
	}
}
