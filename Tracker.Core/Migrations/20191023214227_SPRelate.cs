using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Core.Migrations
{
    public partial class SPRelate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processor",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProcessorID",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProcessorID",
                table: "Projects",
                column: "ProcessorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Staff_ProcessorID",
                table: "Projects",
                column: "ProcessorID",
                principalTable: "Staff",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Staff_ProcessorID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProcessorID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProcessorID",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "Projects",
                nullable: true);
        }
    }
}
