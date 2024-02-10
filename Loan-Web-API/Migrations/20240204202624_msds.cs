using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loan_Web_API.Migrations
{
    /// <inheritdoc />
    public partial class msds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanPeriodInDays",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanPeriodInDays",
                table: "Loans");
        }
    }
}
