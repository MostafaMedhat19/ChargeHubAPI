using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChargeHubAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Identecation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CarCharge = table.Column<int>(type: "int", nullable: false),
                    Esp32BtName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Esp32BtAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StatusNorth = table.Column<int>(type: "int", nullable: true),
                    StatusEast = table.Column<int>(type: "int", nullable: true),
                    StatusSouth = table.Column<int>(type: "int", nullable: true),
                    StatusWest = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Identecation",
                table: "Users",
                column: "Identecation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
