using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFG.Migrations
{
    /// <inheritdoc />
    public partial class updateRFQModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyperAssy",
                table: "RFQ",
                newName: "Eqpa");

            migrationBuilder.RenameColumn(
                name: "PartIdentifier",
                table: "RFQ",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "RFQ",
                newName: "PartIdentifier");

            migrationBuilder.RenameColumn(
                name: "Eqpa",
                table: "RFQ",
                newName: "QtyperAssy");
        }
    }
}
