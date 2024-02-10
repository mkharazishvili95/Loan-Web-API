using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Web_API.Migrations
{
    /// <inheritdoc />
    public partial class _666 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmountPayable",
                table: "Loans",
                newName: "TotalAmountPayableInGEL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmountPayableInGEL",
                table: "Loans",
                newName: "TotalAmountPayable");
        }
    }
}
