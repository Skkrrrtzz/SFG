using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFG.Migrations
{
    /// <inheritdoc />
    public partial class updateRFQModeladdedAnnual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnualForecast",
                table: "RFQ",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualForecast",
                table: "RFQ");
        }
    }
}
