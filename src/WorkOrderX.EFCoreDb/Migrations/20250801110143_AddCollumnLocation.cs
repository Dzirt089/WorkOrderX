using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderX.EFCoreDb.Migrations
{
	/// <inheritdoc />
	public partial class AddCollumnLocation : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "TypeBreakdownId",
				table: "ProcessRequests",
				type: "int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AddColumn<string>(
				name: "Location",
				table: "ProcessRequests",
				type: "nvarchar(max)",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Location",
				table: "ProcessRequests");

			migrationBuilder.AlterColumn<int>(
				name: "TypeBreakdownId",
				table: "ProcessRequests",
				type: "int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);
		}
	}
}
