using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlanPetJourney.Data.Migrations
{
    /// <inheritdoc />
    public partial class policyholderLeadId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "PolicyHolder",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "PolicyHolder");
        }
    }
}
