using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFG.Migrations
{
    /// <inheritdoc />
    public partial class lastpurchaseinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastPurchaseInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    GWRLQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastPurchasedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastPurchasedUSDPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustomerVendorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerVendorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RMWHEREUSED = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FGName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastPurchaseInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastPurchaseInfo");
        }
    }
}
