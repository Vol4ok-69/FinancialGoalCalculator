using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Finance.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContributionFrequencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionFrequencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoalCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HashPassword = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TargetAmount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PriorityId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_GoalCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GoalCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_GoalStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "GoalStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_Priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalGoalContributions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialGoalId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalGoalContributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalGoalContributions_FinancialGoals_FinancialGoalId",
                        column: x => x.FinancialGoalId,
                        principalTable: "FinancialGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialGoalId = table.Column<int>(type: "integer", nullable: false),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    PlannedAmount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalPlans_ContributionFrequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "ContributionFrequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoalPlans_FinancialGoals_FinancialGoalId",
                        column: x => x.FinancialGoalId,
                        principalTable: "FinancialGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContributionFrequencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Без повторения" },
                    { 2, "Ежедневно" },
                    { 3, "Еженедельно" },
                    { 4, "Ежемесячно" }
                });

            migrationBuilder.InsertData(
                table: "GoalCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Покупки" },
                    { 2, "Сбережения" },
                    { 3, "Инвестиции" }
                });

            migrationBuilder.InsertData(
                table: "GoalStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Активна" },
                    { 2, "Достигнута" },
                    { 3, "Провалена" }
                });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Низкий" },
                    { 2, "Средний" },
                    { 3, "Высокий" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_CategoryId",
                table: "FinancialGoals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_PriorityId",
                table: "FinancialGoals",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_StatusId",
                table: "FinancialGoals",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_UserId",
                table: "FinancialGoals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalGoalContributions_FinancialGoalId",
                table: "GoalGoalContributions",
                column: "FinancialGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalPlans_FinancialGoalId",
                table: "GoalPlans",
                column: "FinancialGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalPlans_FrequencyId",
                table: "GoalPlans",
                column: "FrequencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalGoalContributions");

            migrationBuilder.DropTable(
                name: "GoalPlans");

            migrationBuilder.DropTable(
                name: "ContributionFrequencies");

            migrationBuilder.DropTable(
                name: "FinancialGoals");

            migrationBuilder.DropTable(
                name: "GoalCategories");

            migrationBuilder.DropTable(
                name: "GoalStatuses");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
