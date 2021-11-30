using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingCompetitionApi.Migrations
{
    public partial class RemoveLogColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgramFooter",
                table: "CodeSubmissionLogs");

            migrationBuilder.DropColumn(
                name: "ProgramHeader",
                table: "CodeSubmissionLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgramFooter",
                table: "CodeSubmissionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProgramHeader",
                table: "CodeSubmissionLogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
