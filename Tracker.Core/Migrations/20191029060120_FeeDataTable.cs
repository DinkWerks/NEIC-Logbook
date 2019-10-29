using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Core.Migrations
{
    public partial class FeeDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StaffTime = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    HalfStaffTime = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    InHouseTime = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    StaffAssistanceTime = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    GISFeatures = table.Column<int>(nullable: false),
                    IsAddressMappedFee = table.Column<bool>(nullable: false),
                    DBRows = table.Column<int>(nullable: false),
                    QuadsEntered = table.Column<int>(nullable: false),
                    IsPDFFee = table.Column<bool>(nullable: false),
                    PDFPages = table.Column<int>(nullable: false),
                    PrintedPages = table.Column<int>(nullable: false),
                    Adjustment = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    AdjustmentExplanation = table.Column<string>(nullable: true),
                    IsPriority = table.Column<bool>(nullable: false),
                    IsEmergency = table.Column<bool>(nullable: false),
                    IsRapidResponse = table.Column<bool>(nullable: false),
                    ProjectID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeData_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeData_ProjectID",
                table: "FeeData",
                column: "ProjectID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeData");
        }
    }
}
