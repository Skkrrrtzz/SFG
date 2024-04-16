using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFG.Migrations
{
    /// <inheritdoc />
    public partial class updateMRPModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrigManufacturerFinishFabParts",
                table: "RFQ",
                newName: "OrigMPN");

            migrationBuilder.RenameColumn(
                name: "OrigMPNRawMatFab",
                table: "RFQ",
                newName: "OrigMFR");

            migrationBuilder.RenameColumn(
                name: "BOMUOM",
                table: "RFQ",
                newName: "UoM");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualCompletionDate",
                table: "RFQ",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UoM",
                table: "RFQ",
                newName: "BOMUOM");

            migrationBuilder.RenameColumn(
                name: "OrigMPN",
                table: "RFQ",
                newName: "OrigManufacturerFinishFabParts");

            migrationBuilder.RenameColumn(
                name: "OrigMFR",
                table: "RFQ",
                newName: "OrigMPNRawMatFab");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualCompletionDate",
                table: "RFQ",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
