using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecruiterAndJobseekerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobSeeker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Headline = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Summary = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalExperience = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    HighestEducationId = table.Column<int>(type: "int", nullable: true),
                    FieldOfStudyId = table.Column<int>(type: "int", nullable: true),
                    PrimarySkills = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondarySkills = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpenToWork = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CurrentSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ExpectedSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CurrencyCodeId = table.Column<int>(type: "int", nullable: true, defaultValue: 4),
                    IsProfilePublic = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    AllowRecruiterContact = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    MatchScore = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSeeker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSeeker_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobSeeker_Category_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSeeker_Category_FieldOfStudyId",
                        column: x => x.FieldOfStudyId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSeeker_Category_HighestEducationId",
                        column: x => x.HighestEducationId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSeeker_Category_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Recruiter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyWebsite = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IndustryId = table.Column<int>(type: "int", nullable: true),
                    CompanySize = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyDescription = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    IsAgency = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsVerifiedCompany = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ActiveJobs = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    JobsPosted = table.Column<int>(type: "int", nullable: false),
                    TotalHires = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruiter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recruiter_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recruiter_Category_CityId",
                        column: x => x.CityId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recruiter_Category_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recruiter_Category_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recruiter_Category_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_CompanyId",
                table: "JobSeeker",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_FieldOfStudyId",
                table: "JobSeeker",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_HighestEducationId",
                table: "JobSeeker",
                column: "HighestEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_MatchScore",
                table: "JobSeeker",
                column: "MatchScore");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_OpenToWork",
                table: "JobSeeker",
                column: "OpenToWork");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_PositionId",
                table: "JobSeeker",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_UserId",
                table: "JobSeeker",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_CityId",
                table: "Recruiter",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_CompanyId",
                table: "Recruiter",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_CountryId",
                table: "Recruiter",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_IndustryId",
                table: "Recruiter",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_IsActive",
                table: "Recruiter",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_IsAgency",
                table: "Recruiter",
                column: "IsAgency");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiter_UserId",
                table: "Recruiter",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSeeker");

            migrationBuilder.DropTable(
                name: "Recruiter");
        }
    }
}
