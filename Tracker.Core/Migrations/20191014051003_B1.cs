using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Core.Migrations
{
    public partial class B1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_ClientStandings_OrganizationStandingName",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientStandings",
                table: "ClientStandings");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OfficeName",
                table: "Organizations");

            migrationBuilder.RenameTable(
                name: "ClientStandings",
                newName: "OrganizationStandings");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Organizations",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Organizations",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OldPEID",
                table: "Organizations",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewPEID",
                table: "Organizations",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Organizations",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "Organizations",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationStandings",
                table: "OrganizationStandings",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingName",
                table: "Organizations",
                column: "OrganizationStandingName",
                principalTable: "OrganizationStandings",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStandings_OrganizationStandingName",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationStandings",
                table: "OrganizationStandings");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "Organizations");

            migrationBuilder.RenameTable(
                name: "OrganizationStandings",
                newName: "ClientStandings");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OldPEID",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewPEID",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientStandings",
                table: "ClientStandings",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_ClientStandings_OrganizationStandingName",
                table: "Organizations",
                column: "OrganizationStandingName",
                principalTable: "ClientStandings",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
