using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    public partial class MergingCustomUserRolesWithIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys that reference the RoleModels table
            migrationBuilder.DropForeignKey(
                name: "FK_AccountModels_RoleModels_RoleId",
                table: "AccountModels");

            // Drop the old UserModels table
            migrationBuilder.DropTable(
                name: "UserModels");

            // Drop the RoleModels table
            migrationBuilder.DropTable(
                name: "RoleModels");

            // Add a new column to DeviceModels
            migrationBuilder.AddColumn<Guid>(
                name: "SimCardNetworkId",
                table: "DeviceModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // Add the AccountId column to AspNetUsers
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // Create indexes and foreign keys for the new structure
            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_SimCardNetworkId",
                table: "DeviceModels",
                column: "SimCardNetworkId");

            // Establish foreign key relationships using the Identity tables
            migrationBuilder.AddForeignKey(
                name: "FK_AccountModels_AspNetRoles_RoleId",
                table: "AccountModels",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountModels_AccountId",
                table: "AspNetUsers",
                column: "AccountId",
                principalTable: "AccountModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);  // Avoiding multiple cascade paths

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimCardNetworkId",
                table: "DeviceModels",
                column: "SimCardNetworkId",
                principalTable: "SimNetworkModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the foreign key relationships
            migrationBuilder.DropForeignKey(
                name: "FK_AccountModels_AspNetRoles_RoleId",
                table: "AccountModels");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountModels_AccountId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimCardNetworkId",
                table: "DeviceModels");

            // Drop the added column
            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_SimCardNetworkId",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "SimCardNetworkId",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AspNetUsers");

            // Recreate the old RoleModels table
            migrationBuilder.CreateTable(
                name: "RoleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    // Add any other columns as needed
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModels", x => x.Id);
                });

            // Recreate the old UserModels table
            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultiFactor = table.Column<bool>(type: "bit", nullable: false),
                    PreferredMode = table.Column<int>(type: "int", nullable: false),
                    UserTypes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModels_AccountModels_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AccountModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserModels_RoleModels_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RoleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Recreate indexes and foreign keys for the old structure
            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_SimId",
                table: "DeviceModels",
                column: "SimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModels_AccountId",
                table: "UserModels",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModels_RoleId",
                table: "UserModels",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountModels_RoleModels_RoleId",
                table: "AccountModels",
                column: "RoleId",
                principalTable: "RoleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimId",
                table: "DeviceModels",
                column: "SimId",
                principalTable: "SimNetworkModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}