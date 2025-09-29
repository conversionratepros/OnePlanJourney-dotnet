using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlanPetJourney.Data.Migrations
{
    /// <inheritdoc />
    public partial class leadIdBankingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "leadId",
                table: "BankingDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "leadId",
                table: "Address",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "leadId",
                table: "BankingDetails");

            migrationBuilder.DropColumn(
                name: "leadId",
                table: "Address");
        }
    }
}
