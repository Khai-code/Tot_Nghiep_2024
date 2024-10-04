using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class quangdtaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_testQuestions_testCodes_TestCodeId",
                table: "testQuestions");

            migrationBuilder.AlterColumn<Guid>(
                name: "TestCodeId",
                table: "testQuestions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_testQuestions_testCodes_TestCodeId",
                table: "testQuestions",
                column: "TestCodeId",
                principalTable: "testCodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_testQuestions_testCodes_TestCodeId",
                table: "testQuestions");

            migrationBuilder.AlterColumn<Guid>(
                name: "TestCodeId",
                table: "testQuestions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_testQuestions_testCodes_TestCodeId",
                table: "testQuestions",
                column: "TestCodeId",
                principalTable: "testCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
