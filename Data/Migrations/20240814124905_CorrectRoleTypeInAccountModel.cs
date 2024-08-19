using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectRoleTypeInAccountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountModels_AspNetRoles_RoleId",
                table: "AccountModels");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountModels_AspNetRoles_RoleId",
                table: "AccountModels",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountModels_AspNetRoles_RoleId",
                table: "AccountModels");

            migrationBuilder.CreateTable(
                name: "RoleModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityLogPermissions = table.Column<int>(type: "int", nullable: false),
                    AlertPermissions = table.Column<int>(type: "int", nullable: false),
                    ApiAccess = table.Column<bool>(type: "bit", nullable: false),
                    CallPointPermissions = table.Column<int>(type: "int", nullable: false),
                    DashboardAccess = table.Column<bool>(type: "bit", nullable: false),
                    IncomingCallPermissions = table.Column<int>(type: "int", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    PanelSettingsPermissions = table.Column<int>(type: "int", nullable: false),
                    ProgrammingRollBack = table.Column<bool>(type: "bit", nullable: false),
                    PullDeviceProgramming = table.Column<int>(type: "int", nullable: false),
                    RelayOpsPermissions = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmsCommandsPermissions = table.Column<int>(type: "int", nullable: false),
                    TimePeriodPermissions = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountModels_RoleModel_RoleId",
                table: "AccountModels",
                column: "RoleId",
                principalTable: "RoleModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
