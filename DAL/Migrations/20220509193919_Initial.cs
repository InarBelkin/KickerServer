using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    BattleTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BattleTimeSeconds = table.Column<double>(type: "double precision", nullable: false),
                    IsWinnerA = table.Column<bool>(type: "boolean", nullable: false),
                    LoserGoalsCount = table.Column<int>(type: "integer", nullable: false),
                    MarkToDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MarkToDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    FirebaseUuid = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    HashPassword = table.Column<string>(type: "text", nullable: true),
                    RefreshTokens = table.Column<string[]>(type: "text[]", nullable: true),
                    MarkToDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatsOneVsOnes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ELO = table.Column<int>(type: "integer", nullable: false, defaultValue: 1000),
                    BattlesCount = table.Column<int>(type: "integer", nullable: false),
                    WinsCount = table.Column<int>(type: "integer", nullable: false),
                    MarkToDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsOneVsOnes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatsOneVsOnes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatsTwoVsTwos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ELO = table.Column<int>(type: "integer", nullable: false),
                    BattlesCountInAttack = table.Column<int>(type: "integer", nullable: false),
                    WinsCountInAttack = table.Column<int>(type: "integer", nullable: false),
                    BattlesCountInDefense = table.Column<int>(type: "integer", nullable: false),
                    WinsCountInDefense = table.Column<int>(type: "integer", nullable: false),
                    MarkToDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsTwoVsTwos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatsTwoVsTwos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBattles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BattleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Side = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    IsInitiator = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBattles", x => new { x.BattleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBattles_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBattles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthInfos_UserId",
                table: "AuthInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatsOneVsOnes_UserId",
                table: "StatsOneVsOnes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatsTwoVsTwos_UserId",
                table: "StatsTwoVsTwos",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBattles_UserId",
                table: "UserBattles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthInfos");

            migrationBuilder.DropTable(
                name: "StatsOneVsOnes");

            migrationBuilder.DropTable(
                name: "StatsTwoVsTwos");

            migrationBuilder.DropTable(
                name: "UserBattles");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
