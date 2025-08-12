using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderX.EFCoreDb.Migrations
{
	/// <inheritdoc />
	public partial class UpdateTables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ProcessRequests_EquipmentKinds_EquipmentKindId",
				table: "ProcessRequests");

			migrationBuilder.DropForeignKey(
				name: "FK_ProcessRequests_EquipmentModels_EquipmentModelId",
				table: "ProcessRequests");

			migrationBuilder.DropForeignKey(
				name: "FK_ProcessRequests_EquipmentTypes_EquipmentTypeId",
				table: "ProcessRequests");

			migrationBuilder.DropForeignKey(
				name: "FK_TypeBreakdowns_EquipmentTypes_EquipmentTypeId",
				table: "TypeBreakdowns");

			migrationBuilder.DropTable(
				name: "EquipmentKinds");

			migrationBuilder.DropTable(
				name: "EquipmentModels");

			migrationBuilder.DropTable(
				name: "EquipmentTypes");

			migrationBuilder.RenameColumn(
				name: "EquipmentTypeId",
				table: "TypeBreakdowns",
				newName: "InstrumentTypeId");

			migrationBuilder.RenameIndex(
				name: "IX_TypeBreakdowns_EquipmentTypeId",
				table: "TypeBreakdowns",
				newName: "IX_TypeBreakdowns_InstrumentTypeId");

			migrationBuilder.RenameColumn(
				name: "EquipmentTypeId",
				table: "ProcessRequests",
				newName: "InstrumentTypeId");

			migrationBuilder.RenameColumn(
				name: "EquipmentModelId",
				table: "ProcessRequests",
				newName: "InstrumentModelId");

			migrationBuilder.RenameColumn(
				name: "EquipmentKindId",
				table: "ProcessRequests",
				newName: "InstrumentKindId");

			migrationBuilder.RenameIndex(
				name: "IX_ProcessRequests_EquipmentTypeId",
				table: "ProcessRequests",
				newName: "IX_ProcessRequests_InstrumentTypeId");

			migrationBuilder.RenameIndex(
				name: "IX_ProcessRequests_EquipmentModelId",
				table: "ProcessRequests",
				newName: "IX_ProcessRequests_InstrumentModelId");

			migrationBuilder.RenameIndex(
				name: "IX_ProcessRequests_EquipmentKindId",
				table: "ProcessRequests",
				newName: "IX_ProcessRequests_InstrumentKindId");

			migrationBuilder.CreateTable(
				name: "InstrumentModels",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_InstrumentModels", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "InstrumentTypes",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_InstrumentTypes", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "InstrumentKinds",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					InstrumentTypeId = table.Column<int>(type: "int", nullable: false),
					Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_InstrumentKinds", x => x.Id);
					table.ForeignKey(
						name: "FK_InstrumentKinds_InstrumentTypes_InstrumentTypeId",
						column: x => x.InstrumentTypeId,
						principalTable: "InstrumentTypes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_InstrumentKinds_InstrumentTypeId",
				table: "InstrumentKinds",
				column: "InstrumentTypeId");

			// Add FKs with NOCHECK to bypass integrity check on existing data
			migrationBuilder.Sql(
				@"ALTER TABLE [ProcessRequests] WITH NOCHECK ADD CONSTRAINT [FK_ProcessRequests_InstrumentKinds_InstrumentKindId] FOREIGN KEY ([InstrumentKindId]) REFERENCES [InstrumentKinds] ([Id]) ON DELETE NO ACTION;");

			migrationBuilder.Sql(
				@"ALTER TABLE [ProcessRequests] WITH NOCHECK ADD CONSTRAINT [FK_ProcessRequests_InstrumentModels_InstrumentModelId] FOREIGN KEY ([InstrumentModelId]) REFERENCES [InstrumentModels] ([Id]) ON DELETE NO ACTION;");

			migrationBuilder.Sql(
				@"ALTER TABLE [ProcessRequests] WITH NOCHECK ADD CONSTRAINT [FK_ProcessRequests_InstrumentTypes_InstrumentTypeId] FOREIGN KEY ([InstrumentTypeId]) REFERENCES [InstrumentTypes] ([Id]) ON DELETE NO ACTION;");

			migrationBuilder.Sql(
				@"ALTER TABLE [TypeBreakdowns] WITH NOCHECK ADD CONSTRAINT [FK_TypeBreakdowns_InstrumentTypes_InstrumentTypeId] FOREIGN KEY ([InstrumentTypeId]) REFERENCES [InstrumentTypes] ([Id]) ON DELETE NO ACTION;");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ProcessRequests_InstrumentKinds_InstrumentKindId",
				table: "ProcessRequests");

			migrationBuilder.DropForeignKey(
				name: "FK_ProcessRequests_InstrumentModels_InstrumentModelId",
				table: "ProcessRequests");

			migrationBuilder.DropForeignKey(
				name: "FK_ProcessRequests_InstrumentTypes_InstrumentTypeId",
				table: "ProcessRequests");

			migrationBuilder.DropForeignKey(
				name: "FK_TypeBreakdowns_InstrumentTypes_InstrumentTypeId",
				table: "TypeBreakdowns");

			migrationBuilder.DropTable(
				name: "InstrumentKinds");

			migrationBuilder.DropTable(
				name: "InstrumentModels");

			migrationBuilder.DropTable(
				name: "InstrumentTypes");

			migrationBuilder.RenameColumn(
				name: "InstrumentTypeId",
				table: "TypeBreakdowns",
				newName: "EquipmentTypeId");

			migrationBuilder.RenameIndex(
				name: "IX_TypeBreakdowns_InstrumentTypeId",
				table: "TypeBreakdowns",
				newName: "IX_TypeBreakdowns_EquipmentTypeId");

			migrationBuilder.RenameColumn(
				name: "InstrumentTypeId",
				table: "ProcessRequests",
				newName: "EquipmentTypeId");

			migrationBuilder.RenameColumn(
				name: "InstrumentModelId",
				table: "ProcessRequests",
				newName: "EquipmentModelId");

			migrationBuilder.RenameColumn(
				name: "InstrumentKindId",
				table: "ProcessRequests",
				newName: "EquipmentKindId");

			migrationBuilder.RenameIndex(
				name: "IX_ProcessRequests_InstrumentTypeId",
				table: "ProcessRequests",
				newName: "IX_ProcessRequests_EquipmentTypeId");

			migrationBuilder.RenameIndex(
				name: "IX_ProcessRequests_InstrumentModelId",
				table: "ProcessRequests",
				newName: "IX_ProcessRequests_EquipmentModelId");

			migrationBuilder.RenameIndex(
				name: "IX_ProcessRequests_InstrumentKindId",
				table: "ProcessRequests",
				newName: "IX_ProcessRequests_EquipmentKindId");

			migrationBuilder.CreateTable(
				name: "EquipmentModels",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_EquipmentModels", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "EquipmentTypes",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "EquipmentKinds",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
					Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_EquipmentKinds", x => x.Id);
					table.ForeignKey(
						name: "FK_EquipmentKinds_EquipmentTypes_EquipmentTypeId",
						column: x => x.EquipmentTypeId,
						principalTable: "EquipmentTypes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_EquipmentKinds_EquipmentTypeId",
				table: "EquipmentKinds",
				column: "EquipmentTypeId");

			// Add FKs with NOCHECK for rollback (same issue in Down)
			migrationBuilder.Sql(
				@"ALTER TABLE [ProcessRequests] WITH NOCHECK ADD CONSTRAINT [FK_ProcessRequests_EquipmentKinds_EquipmentKindId] FOREIGN KEY ([EquipmentKindId]) REFERENCES [EquipmentKinds] ([Id]) ON DELETE NO ACTION;");

			migrationBuilder.Sql(
				@"ALTER TABLE [ProcessRequests] WITH NOCHECK ADD CONSTRAINT [FK_ProcessRequests_EquipmentModels_EquipmentModelId] FOREIGN KEY ([EquipmentModelId]) REFERENCES [EquipmentModels] ([Id]) ON DELETE NO ACTION;");

			migrationBuilder.Sql(
				@"ALTER TABLE [ProcessRequests] WITH NOCHECK ADD CONSTRAINT [FK_ProcessRequests_EquipmentTypes_EquipmentTypeId] FOREIGN KEY ([EquipmentTypeId]) REFERENCES [EquipmentTypes] ([Id]) ON DELETE NO ACTION;");

			migrationBuilder.Sql(
				@"ALTER TABLE [TypeBreakdowns] WITH NOCHECK ADD CONSTRAINT [FK_TypeBreakdowns_EquipmentTypes_EquipmentTypeId] FOREIGN KEY ([EquipmentTypeId]) REFERENCES [EquipmentTypes] ([Id]) ON DELETE NO ACTION;");
		}
	}
}