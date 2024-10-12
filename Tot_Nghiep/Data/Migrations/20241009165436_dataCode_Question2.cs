using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class dataCode_Question2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_TestQuestion_testCodes_TestCodeId",
                table: "TestCode_TestQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_TestQuestion_testQuestions_TestQuestionId",
                table: "TestCode_TestQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestCode_TestQuestion",
                table: "TestCode_TestQuestion");

            migrationBuilder.RenameTable(
                name: "TestCode_TestQuestion",
                newName: "TestCode_TestQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_TestCode_TestQuestion_TestQuestionId",
                table: "TestCode_TestQuestions",
                newName: "IX_TestCode_TestQuestions_TestQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_TestCode_TestQuestion_TestCodeId",
                table: "TestCode_TestQuestions",
                newName: "IX_TestCode_TestQuestions_TestCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestCode_TestQuestions",
                table: "TestCode_TestQuestions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_TestQuestions_testCodes_TestCodeId",
                table: "TestCode_TestQuestions",
                column: "TestCodeId",
                principalTable: "testCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_TestQuestions_testQuestions_TestQuestionId",
                table: "TestCode_TestQuestions",
                column: "TestQuestionId",
                principalTable: "testQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_TestQuestions_testCodes_TestCodeId",
                table: "TestCode_TestQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_TestQuestions_testQuestions_TestQuestionId",
                table: "TestCode_TestQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestCode_TestQuestions",
                table: "TestCode_TestQuestions");

            migrationBuilder.RenameTable(
                name: "TestCode_TestQuestions",
                newName: "TestCode_TestQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_TestCode_TestQuestions_TestQuestionId",
                table: "TestCode_TestQuestion",
                newName: "IX_TestCode_TestQuestion_TestQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_TestCode_TestQuestions_TestCodeId",
                table: "TestCode_TestQuestion",
                newName: "IX_TestCode_TestQuestion_TestCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestCode_TestQuestion",
                table: "TestCode_TestQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_TestQuestion_testCodes_TestCodeId",
                table: "TestCode_TestQuestion",
                column: "TestCodeId",
                principalTable: "testCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_TestQuestion_testQuestions_TestQuestionId",
                table: "TestCode_TestQuestion",
                column: "TestQuestionId",
                principalTable: "testQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
