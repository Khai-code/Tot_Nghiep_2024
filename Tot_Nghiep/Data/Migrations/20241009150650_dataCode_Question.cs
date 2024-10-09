using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class dataCode_Question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestCode_TestQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCode_TestQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCode_TestQuestion_testCodes_TestCodeId",
                        column: x => x.TestCodeId,
                        principalTable: "testCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestCode_TestQuestion_testQuestions_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "testQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCode_TestQuestion_TestCodeId",
                table: "TestCode_TestQuestion",
                column: "TestCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCode_TestQuestion_TestQuestionId",
                table: "TestCode_TestQuestion",
                column: "TestQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestCode_TestQuestion");
        }
    }
}
