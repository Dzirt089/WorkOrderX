using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderX.EFCoreDb.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypes", x => x.Id);
                });

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
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specializeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TypeBreakdowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeBreakdowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeBreakdowns_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkplaceEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Account = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    SpecializedId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkplaceEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkplaceEmployees_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkplaceEmployees_Specializeds_SpecializedId",
                        column: x => x.SpecializedId,
                        principalTable: "Specializeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DescriptionMalfunction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutorEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: true),
                    EquipmentKindId = table.Column<int>(type: "int", nullable: true),
                    TypeBreakdownId = table.Column<int>(type: "int", nullable: false),
                    EquipmentModelId = table.Column<int>(type: "int", nullable: true),
                    ApplicationStatusId = table.Column<int>(type: "int", nullable: false),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_ApplicationStatuses_ApplicationStatusId",
                        column: x => x.ApplicationStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_ApplicationTypes_ApplicationTypeId",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_EquipmentKinds_EquipmentKindId",
                        column: x => x.EquipmentKindId,
                        principalTable: "EquipmentKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_EquipmentModels_EquipmentModelId",
                        column: x => x.EquipmentModelId,
                        principalTable: "EquipmentModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_TypeBreakdowns_TypeBreakdownId",
                        column: x => x.TypeBreakdownId,
                        principalTable: "TypeBreakdowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_WorkplaceEmployees_CustomerEmployeeId",
                        column: x => x.CustomerEmployeeId,
                        principalTable: "WorkplaceEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_WorkplaceEmployees_ExecutorEmployeeId",
                        column: x => x.ExecutorEmployeeId,
                        principalTable: "WorkplaceEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventStoreEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldStatusId = table.Column<int>(type: "int", nullable: true),
                    NewStatusId = table.Column<int>(type: "int", nullable: false),
                    ChangedByEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExecutorEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStoreEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventStoreEntries_ApplicationStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventStoreEntries_ApplicationStatuses_OldStatusId",
                        column: x => x.OldStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventStoreEntries_ProcessRequests_AggregateId",
                        column: x => x.AggregateId,
                        principalTable: "ProcessRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventStoreEntries_WorkplaceEmployees_ChangedByEmployeeId",
                        column: x => x.ChangedByEmployeeId,
                        principalTable: "WorkplaceEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventStoreEntries_WorkplaceEmployees_CustomerEmployeeId",
                        column: x => x.CustomerEmployeeId,
                        principalTable: "WorkplaceEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventStoreEntries_WorkplaceEmployees_ExecutorEmployeeId",
                        column: x => x.ExecutorEmployeeId,
                        principalTable: "WorkplaceEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentKinds_EquipmentTypeId",
                table: "EquipmentKinds",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_AggregateId_CustomerEmployeeId",
                table: "EventStoreEntries",
                columns: new[] { "AggregateId", "CustomerEmployeeId" })
                .Annotation("SqlServer:Include", new[] { "ExecutorEmployeeId", "OldStatusId", "NewStatusId", "Comment", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_ChangedByEmployeeId",
                table: "EventStoreEntries",
                column: "ChangedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_CustomerEmployeeId",
                table: "EventStoreEntries",
                column: "CustomerEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_ExecutorEmployeeId",
                table: "EventStoreEntries",
                column: "ExecutorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_NewStatusId",
                table: "EventStoreEntries",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_OccurredAt",
                table: "EventStoreEntries",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_EventStoreEntries_OldStatusId",
                table: "EventStoreEntries",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_ApplicationNumber",
                table: "ProcessRequests",
                column: "ApplicationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_ApplicationStatusId",
                table: "ProcessRequests",
                column: "ApplicationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_ApplicationTypeId",
                table: "ProcessRequests",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_CreatedAt",
                table: "ProcessRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_CustomerEmployeeId",
                table: "ProcessRequests",
                column: "CustomerEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_EquipmentKindId",
                table: "ProcessRequests",
                column: "EquipmentKindId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_EquipmentModelId",
                table: "ProcessRequests",
                column: "EquipmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_EquipmentTypeId",
                table: "ProcessRequests",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_ExecutorEmployeeId",
                table: "ProcessRequests",
                column: "ExecutorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_PlannedAt",
                table: "ProcessRequests",
                column: "PlannedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_SerialNumber",
                table: "ProcessRequests",
                column: "SerialNumber",
                unique: true,
                filter: "[SerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_TypeBreakdownId",
                table: "ProcessRequests",
                column: "TypeBreakdownId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeBreakdowns_EquipmentTypeId",
                table: "TypeBreakdowns",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkplaceEmployee_Email",
                table: "WorkplaceEmployees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkplaceEmployees_Account",
                table: "WorkplaceEmployees",
                column: "Account",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkplaceEmployees_RoleId",
                table: "WorkplaceEmployees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkplaceEmployees_SpecializedId",
                table: "WorkplaceEmployees",
                column: "SpecializedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventStoreEntries");

            migrationBuilder.DropTable(
                name: "ProcessRequests");

            migrationBuilder.DropTable(
                name: "ApplicationStatuses");

            migrationBuilder.DropTable(
                name: "ApplicationTypes");

            migrationBuilder.DropTable(
                name: "EquipmentKinds");

            migrationBuilder.DropTable(
                name: "EquipmentModels");

            migrationBuilder.DropTable(
                name: "TypeBreakdowns");

            migrationBuilder.DropTable(
                name: "WorkplaceEmployees");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Specializeds");
        }
    }
}
