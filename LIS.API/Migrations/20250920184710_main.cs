using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APiUsers.Migrations
{
    public partial class main : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SettingSystem",
                columns: table => new
                {
                    SettingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Addrees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descraption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingSystem", x => x.SettingID);
                });

            migrationBuilder.CreateTable(
                name: "TestCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(500)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 20, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    TestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TestNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SampleType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Testprice = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Test_TestCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TestCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_patients_users_SupervisorID",
                        column: x => x.SupervisorID,
                        principalTable: "users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "RecordPatients",
                columns: table => new
                {
                    RecurdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TechnicianiD = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordPatients", x => x.RecurdId);
                    table.ForeignKey(
                        name: "FK_RecordPatients_patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recuests",
                columns: table => new
                {
                    RecuestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recuests", x => x.RecuestID);
                    table.ForeignKey(
                        name: "FK_recuests_patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recuests_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestTest",
                columns: table => new
                {
                    RequestTestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    TestID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTest", x => x.RequestTestID);
                    table.ForeignKey(
                        name: "FK_RequestTest_recuests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "recuests",
                        principalColumn: "RecuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestTest_Test_TestID",
                        column: x => x.TestID,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecordRequestTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    RequestTestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordRequestTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordRequestTests_RecordPatients_RecordId",
                        column: x => x.RecordId,
                        principalTable: "RecordPatients",
                        principalColumn: "RecurdId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordRequestTests_RequestTest_RequestTestId",
                        column: x => x.RequestTestId,
                        principalTable: "RequestTest",
                        principalColumn: "RequestTestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    ResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestTestID = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    ResultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LabTechniciansUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.ResultID);
                    table.ForeignKey(
                        name: "FK_TestResults_RequestTest_RequestTestID",
                        column: x => x.RequestTestID,
                        principalTable: "RequestTest",
                        principalColumn: "RequestTestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestResults_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestResults_users_LabTechniciansUserID",
                        column: x => x.LabTechniciansUserID,
                        principalTable: "users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_patients_SupervisorID",
                table: "patients",
                column: "SupervisorID");

            migrationBuilder.CreateIndex(
                name: "IX_RecordPatients_PatientID",
                table: "RecordPatients",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_RecordRequestTests_RecordId",
                table: "RecordRequestTests",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordRequestTests_RequestTestId",
                table: "RecordRequestTests",
                column: "RequestTestId");

            migrationBuilder.CreateIndex(
                name: "IX_recuests_PatientID",
                table: "recuests",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_recuests_UserID",
                table: "recuests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTest_RequestID",
                table: "RequestTest",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTest_TestID",
                table: "RequestTest",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_CategoryId",
                table: "Test",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_LabTechniciansUserID",
                table: "TestResults",
                column: "LabTechniciansUserID");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_RequestTestID",
                table: "TestResults",
                column: "RequestTestID");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestId",
                table: "TestResults",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordRequestTests");

            migrationBuilder.DropTable(
                name: "SettingSystem");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "RecordPatients");

            migrationBuilder.DropTable(
                name: "RequestTest");

            migrationBuilder.DropTable(
                name: "recuests");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "TestCategories");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
