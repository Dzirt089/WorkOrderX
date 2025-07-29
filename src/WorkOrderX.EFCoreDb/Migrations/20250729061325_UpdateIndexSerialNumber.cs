using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderX.EFCoreDb.Migrations
{
	/// <inheritdoc />
	public partial class UpdateIndexSerialNumber : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_ProcessRequests_SerialNumber",
				table: "ProcessRequests");

			migrationBuilder.CreateIndex(
				name: "IX_ProcessRequests_SerialNumber",
				table: "ProcessRequests",
				column: "SerialNumber");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_ProcessRequests_SerialNumber",
				table: "ProcessRequests");

			migrationBuilder.CreateIndex(
				name: "IX_ProcessRequests_SerialNumber",
				table: "ProcessRequests",
				column: "SerialNumber",
				unique: true,
				filter: "[SerialNumber] IS NOT NULL");
		}
	}
}
