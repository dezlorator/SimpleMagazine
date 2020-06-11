using Microsoft.EntityFrameworkCore.Migrations;

namespace _3Lab.Migrations
{
    public partial class RolesImplimented : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanAddComments = table.Column<bool>(nullable: false),
                    CanModerateComments = table.Column<bool>(nullable: false),
                    CanEditProducts = table.Column<bool>(nullable: false),
                    CanPurchaseToStock = table.Column<bool>(nullable: false),
                    CanDeleteProducts = table.Column<bool>(nullable: false),
                    CanAddProducts = table.Column<bool>(nullable: false),
                    CanViewStatistics = table.Column<bool>(nullable: false),
                    CanViewUsersList = table.Column<bool>(nullable: false),
                    CanSetRoles = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRole_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRole_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");
        }
    }
}
