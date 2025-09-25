using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlanPetJourney.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeAddressmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAddresses");

            migrationBuilder.DropTable(
                name: "PhysicalAddresses");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeliveryAddressLineOne = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryAddressLineTwo = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryCity = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryPostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    PhysicalAddressLineOne = table.Column<string>(type: "TEXT", nullable: false),
                    PhysicalAddressLineTwo = table.Column<string>(type: "TEXT", nullable: true),
                    PhysicalCity = table.Column<string>(type: "TEXT", nullable: false),
                    PhysicalPostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.CreateTable(
                name: "DeliveryAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressLineOne = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLineTwo = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressLineOne = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLineTwo = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalAddresses", x => x.Id);
                });
        }
    }
}
