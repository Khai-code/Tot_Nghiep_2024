using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class RenameFieldNumberOfQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfQuestion",
                table: "tests",
                newName: "NumberOfTestCode");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "tests");

            migrationBuilder.RenameColumn(
                name: "NumberOfTestCode",
                table: "tests",
                newName: "NumberOfQuestion");
        }
    }
}
