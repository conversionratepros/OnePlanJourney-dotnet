using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlanPetJourney.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBankingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    isPersonal = table.Column<bool>(type: "INTEGER", nullable: false),
                    isPassport = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccountPayersID = table.Column<string>(type: "TEXT", nullable: false),
                    AccountHolder = table.Column<string>(type: "TEXT", nullable: false),
                    AccountNumber = table.Column<string>(type: "TEXT", nullable: false),
                    BankName = table.Column<string>(type: "TEXT", nullable: false),
                    BranchCode = table.Column<string>(type: "TEXT", nullable: false),
                    BankAccountType = table.Column<string>(type: "TEXT", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PolicyStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SecondStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HearAboutUs = table.Column<string>(type: "TEXT", nullable: false),
                    ConfirmPhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ConsentConfirm = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankingDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankingDetails");
        }
    }
}
