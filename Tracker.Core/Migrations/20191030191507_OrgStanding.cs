using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Core.Migrations
{
    public partial class OrgStanding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingName",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationStandings",
                table: "OrganizationStandings");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationStandingName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationStandingName",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganizationStandings",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrganizationStandings",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationStandingId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationStandings",
                table: "OrganizationStandings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationStandingId",
                table: "Organizations",
                column: "OrganizationStandingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingId",
                table: "Organizations",
                column: "OrganizationStandingId",
                principalTable: "OrganizationStandings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingId",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationStandings",
                table: "OrganizationStandings");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationStandingId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganizationStandings");

            migrationBuilder.DropColumn(
                name: "OrganizationStandingId",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganizationStandings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationStandingName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationStandings",
                table: "OrganizationStandings",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationStandingName",
                table: "Organizations",
                column: "OrganizationStandingName");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingName",
                table: "Organizations",
                column: "OrganizationStandingName",
                principalTable: "OrganizationStandings",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
