using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistory_AspNetUsers_PerformedByUserId",
                table: "ActionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Fines_AspNetUsers_ReceivedByUserId",
                table: "Fines");

            migrationBuilder.DropForeignKey(
                name: "FK_Fines_AspNetUsers_UserId",
                table: "Fines");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_CheckedOutByUserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_ReturnedByUserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_UserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_UserId1",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_CheckedOutByUserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_ReturnedByUserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId1",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Fines_ReceivedByUserId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Fines_UserId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_ActionHistory_PerformedByUserId",
                table: "ActionHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Fines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Fines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CheckedOutByUserId",
                table: "Loans",
                column: "CheckedOutByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ReturnedByUserId",
                table: "Loans",
                column: "ReturnedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId1",
                table: "Loans",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Fines_ReceivedByUserId",
                table: "Fines",
                column: "ReceivedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fines_UserId",
                table: "Fines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionHistory_PerformedByUserId",
                table: "ActionHistory",
                column: "PerformedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistory_AspNetUsers_PerformedByUserId",
                table: "ActionHistory",
                column: "PerformedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_AspNetUsers_ReceivedByUserId",
                table: "Fines",
                column: "ReceivedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_AspNetUsers_UserId",
                table: "Fines",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_CheckedOutByUserId",
                table: "Loans",
                column: "CheckedOutByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_ReturnedByUserId",
                table: "Loans",
                column: "ReturnedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_UserId1",
                table: "Loans",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
