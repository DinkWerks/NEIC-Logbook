using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Core.Migrations
{
    public partial class OrgStanding2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationStandingId",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingId",
                table: "Organizations",
                column: "OrganizationStandingId",
                principalTable: "OrganizationStandings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationStandingId",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingId",
                table: "Organizations",
                column: "OrganizationStandingId",
                principalTable: "OrganizationStandings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
