using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlanPetJourney.Data.Migrations
{
    /// <inheritdoc />
    public partial class Commit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "Pets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_LeadId",
                table: "Pets",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Leads_LeadId",
                table: "Pets",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Leads_LeadId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_LeadId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Pets");
        }
    }
}
