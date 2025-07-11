using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.EFCoreDb.ModelsConfigurations
{
	public class WorkplaceEmployeeConfiguration : IEntityTypeConfiguration<WorkplaceEmployee>
	{
		public void Configure(EntityTypeBuilder<WorkplaceEmployee> builder)
		{
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedOnAdd()
				.HasDefaultValueSql("newsequentialid");

			//Value Object как Owned Types
			builder.OwnsOne(_ => _.Account, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(Account))
					.IsRequired();

				_.HasIndex(_ => _.Value)
					.IsUnique()
					.HasDatabaseName("IX_WorkplaceEmployee_Account");
			});

			builder.OwnsOne(_ => _.Name, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(Name))
					.IsRequired();
			});

			builder.OwnsOne(_ => _.Department, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(Department))
					.IsRequired();
			});

			builder.OwnsOne(_ => _.Email, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(Email))
					.IsRequired();

				_.HasIndex(_ => _.Value)
					.IsUnique()
					.HasDatabaseName("IX_WorkplaceEmployee_Email");
			});

			builder.OwnsOne(_ => _.Phone, _ =>
			{
				_.Property(_ => _.Value)
					.HasColumnName(nameof(Phone))
					.IsRequired();
			});


			// Связи с Enumeration (справочные таблицы)
			builder.HasOne(_ => _.Role) // у WorkplaceEmployee может быть одна Role.
				.WithMany() // у Role может быть много WorkplaceEmployee.
				.HasForeignKey(_ => _.RoleId)
				.IsRequired();

			builder.HasOne(_ => _.Specialized) // у WorkplaceEmployee может быть одна Specialized.
				.WithMany() // у Specialized может быть много WorkplaceEmployee.
				.HasForeignKey(_ => _.SpecializedId)
				.IsRequired(false);
		}
	}
}
