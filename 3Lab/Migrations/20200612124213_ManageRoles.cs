using Microsoft.EntityFrameworkCore.Migrations;

namespace _3Lab.Migrations
{
    public partial class ManageRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanManageOrders",
                table: "UserRole",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManageOrders",
                table: "UserRole");
        }
    }
}
