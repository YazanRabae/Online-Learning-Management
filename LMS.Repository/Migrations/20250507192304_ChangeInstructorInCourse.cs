using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInstructorInCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_AspNetUsers_InstructorId1",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_InstructorId1",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "InstructorId1",
                table: "Course");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId1",
                table: "Course",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_InstructorId1",
                table: "Course",
                column: "InstructorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_AspNetUsers_InstructorId1",
                table: "Course",
                column: "InstructorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
