using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Models.EventStores;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class EventStoreEntryConfiguration : IEntityTypeConfiguration<EventStoreEntry>
	{
		public void Configure(EntityTypeBuilder<EventStoreEntry> builder)
		{
			builder.ToTable("EventStoreEntries");
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedOnAdd();

			builder.HasOne<ProcessRequest>()
				.WithMany()
				.HasForeignKey(_ => _.AggregateId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.CustomerEmployeeId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.ExecutorEmployeeId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<WorkplaceEmployee>()
				.WithMany()
				.HasForeignKey(_ => _.ChangedByEmployeeId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<ApplicationStatus>()
				.WithMany()
				.HasForeignKey(_ => _.OldStatusId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<ApplicationStatus>()
				.WithMany()
				.HasForeignKey(_ => _.NewStatusId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(_ => _.Comment)
				.IsRequired(false);

			builder.HasOne<Importance>()
				.WithMany()
				.HasForeignKey(_ => _.ImportanceId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(_ => new { _.AggregateId, _.CustomerEmployeeId })
				.IncludeProperties(_ => new
				{
					_.ExecutorEmployeeId,
					_.OldStatusId,
					_.NewStatusId,
					_.Comment,
					_.OccurredAt,
					_.ImportanceId,
				});

			builder.HasIndex(_ => _.OccurredAt);
		}
	}
}
