using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Core.Migrations
{
    public partial class Test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientStandings",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    Severity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStandings", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OldPEID = table.Column<string>(nullable: true),
                    NewPEID = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    OfficeName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    OrganizationStandingName = table.Column<string>(nullable: true),
                    AddressID = table.Column<int>(nullable: false),
                    Address_AddressName = table.Column<string>(nullable: true),
                    Address_AttentionTo = table.Column<string>(nullable: true),
                    Address_AddressLine1 = table.Column<string>(nullable: true),
                    Address_AddressLine2 = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_State = table.Column<string>(nullable: true),
                    Address_ZIP = table.Column<string>(nullable: true),
                    Address_Notes = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Organizations_ClientStandings_OrganizationStandingName",
                        column: x => x.OrganizationStandingName,
                        principalTable: "ClientStandings",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    AffiliationID = table.Column<int>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DisclosureLevel = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    AddressID = table.Column<int>(nullable: false),
                    Address_AddressName = table.Column<string>(nullable: true),
                    Address_AttentionTo = table.Column<string>(nullable: true),
                    Address_AddressLine1 = table.Column<string>(nullable: true),
                    Address_AddressLine2 = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_State = table.Column<string>(nullable: true),
                    Address_ZIP = table.Column<string>(nullable: true),
                    Address_Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.ID);
                    table.ForeignKey(
                        name: "FK_People_Organizations_AffiliationID",
                        column: x => x.AffiliationID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ICTypePrefix = table.Column<string>(maxLength: 1, nullable: true),
                    ICYear = table.Column<string>(maxLength: 2, nullable: true),
                    ICEnumeration = table.Column<int>(nullable: false),
                    ICSuffix = table.Column<string>(maxLength: 3, nullable: true),
                    DateReceived = table.Column<DateTime>(nullable: true),
                    DateEntered = table.Column<DateTime>(nullable: true),
                    DateOfResponse = table.Column<DateTime>(nullable: true),
                    DateBilled = table.Column<DateTime>(nullable: true),
                    DatePaid = table.Column<DateTime>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    RequestorID = table.Column<int>(nullable: true),
                    AdditionalRequestors = table.Column<string>(nullable: true),
                    ClientID = table.Column<int>(nullable: true),
                    MailingAddress_AddressName = table.Column<string>(nullable: true),
                    MailingAddress_AttentionTo = table.Column<string>(nullable: true),
                    MailingAddress_AddressLine1 = table.Column<string>(nullable: true),
                    MailingAddress_AddressLine2 = table.Column<string>(nullable: true),
                    MailingAddress_City = table.Column<string>(nullable: true),
                    MailingAddress_State = table.Column<string>(nullable: true),
                    MailingAddress_ZIP = table.Column<string>(nullable: true),
                    MailingAddress_Notes = table.Column<string>(nullable: true),
                    BillingAddress_AddressName = table.Column<string>(nullable: true),
                    BillingAddress_AttentionTo = table.Column<string>(nullable: true),
                    BillingAddress_AddressLine1 = table.Column<string>(nullable: true),
                    BillingAddress_AddressLine2 = table.Column<string>(nullable: true),
                    BillingAddress_City = table.Column<string>(nullable: true),
                    BillingAddress_State = table.Column<string>(nullable: true),
                    BillingAddress_ZIP = table.Column<string>(nullable: true),
                    BillingAddress_Notes = table.Column<string>(nullable: true),
                    IsMailingSameAsBilling = table.Column<bool>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectType = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<string>(maxLength: 100, nullable: true),
                    SpecialDetails = table.Column<string>(nullable: true),
                    MainCounty = table.Column<string>(nullable: true),
                    AdditionalCounties = table.Column<string>(nullable: true),
                    PLSS = table.Column<string>(nullable: true),
                    Acres = table.Column<decimal>(nullable: false),
                    LinearMiles = table.Column<decimal>(nullable: false),
                    AreResourcesInProject = table.Column<bool>(nullable: false),
                    Recommendation = table.Column<string>(maxLength: 1000, nullable: true),
                    IsReportReceived = table.Column<bool>(nullable: false),
                    EncryptionPassword = table.Column<string>(nullable: true),
                    Processor = table.Column<string>(nullable: true),
                    FeeVersion = table.Column<string>(maxLength: 50, nullable: true),
                    FeeID = table.Column<int>(nullable: false),
                    IsPrePaid = table.Column<bool>(nullable: false),
                    TotalFee = table.Column<decimal>(nullable: false),
                    ProjectNumber = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CheckName = table.Column<string>(maxLength: 100, nullable: true),
                    CheckNumber = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_People_RequestorID",
                        column: x => x.RequestorID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationStandingName",
                table: "Organizations",
                column: "OrganizationStandingName");

            migrationBuilder.CreateIndex(
                name: "IX_People_AffiliationID",
                table: "People",
                column: "AffiliationID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientID",
                table: "Projects",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RequestorID",
                table: "Projects",
                column: "RequestorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "ClientStandings");
        }
    }
}
