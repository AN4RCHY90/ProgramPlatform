using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    public partial class TweaksToAppDbContextToHelpWithMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountModels_AccountId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimCardNetworkId",
                table: "DeviceModels");

            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_SimCardNetworkId",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "SimCardNetworkId",
                table: "DeviceModels");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DeviceModels_SimId' AND object_id = OBJECT_ID('DeviceModels'))
                BEGIN
                    CREATE INDEX [IX_DeviceModels_SimId] ON [DeviceModels] ([SimId]);
                END
            ");

            // Drop the existing foreign key constraint if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DeviceModels_SimNetworkModels_SimId')
                BEGIN
                    ALTER TABLE [DeviceModels] DROP CONSTRAINT [FK_DeviceModels_SimNetworkModels_SimId];
                END
            ");

            // Add the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimId",
                table: "DeviceModels",
                column: "SimId",
                principalTable: "SimNetworkModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountModels_AccountId",
                table: "AspNetUsers",
                column: "AccountId",
                principalTable: "AccountModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountModels_AccountId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimId",
                table: "DeviceModels");

            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_SimId",
                table: "DeviceModels");

            migrationBuilder.AddColumn<Guid>(
                name: "SimCardNetworkId",
                table: "DeviceModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_SimCardNetworkId",
                table: "DeviceModels",
                column: "SimCardNetworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountModels_AccountId",
                table: "AspNetUsers",
                column: "AccountId",
                principalTable: "AccountModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_SimNetworkModels_SimCardNetworkId",
                table: "DeviceModels",
                column: "SimCardNetworkId",
                principalTable: "SimNetworkModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
