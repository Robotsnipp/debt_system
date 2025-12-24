using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "Debts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "Debts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
