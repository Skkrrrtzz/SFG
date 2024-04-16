using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFG.Migrations
{
    /// <inheritdoc />
    public partial class addednewRFQProjectsandupdateRFQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoItems",
                table: "RFQ");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "RFQ");

            migrationBuilder.DropColumn(
                name: "RequiredDate",
                table: "RFQ");

            migrationBuilder.RenameColumn(
                name: "ActualCompletionDate",
                table: "RFQ",
                newName: "LastPurchaseDate");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "RFQ",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RFQProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoItems = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFQProjects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RFQProjects");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "RFQ");

            migrationBuilder.RenameColumn(
                name: "LastPurchaseDate",
                table: "RFQ",
                newName: "ActualCompletionDate");

            migrationBuilder.AddColumn<int>(
                name: "NoItems",
                table: "RFQ",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "RFQ",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredDate",
                table: "RFQ",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
