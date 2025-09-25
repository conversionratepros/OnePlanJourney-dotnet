using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlanPetJourney.Data.Migrations
{
    /// <inheritdoc />
    public partial class policyHolder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyHolder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    MobileNumber = table.Column<string>(type: "TEXT", nullable: false),
                    AltMobileNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    hasConcented = table.Column<bool>(type: "INTEGER", nullable: false),
                    marketing = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyHolder", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyHolder");
        }
    }
}
