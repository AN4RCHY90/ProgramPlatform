using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatingModelTablesAndTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceUpdateSettingsModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfRetries = table.Column<int>(type: "int", nullable: false),
                    RetryInterval = table.Column<int>(type: "int", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProgrammingMethodThresold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceUpdateSettingsModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OAuthTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    PullDeviceProgramming = table.Column<int>(type: "int", nullable: false),
                    ApiAccess = table.Column<bool>(type: "bit", nullable: false),
                    DashboardAccess = table.Column<bool>(type: "bit", nullable: false),
                    ProgrammingRollBack = table.Column<bool>(type: "bit", nullable: false),
                    ActivityLogPermissions = table.Column<int>(type: "int", nullable: false),
                    CallPointPermissions = table.Column<int>(type: "int", nullable: false),
                    IncomingCallPermissions = table.Column<int>(type: "int", nullable: false),
                    TimePeriodPermissions = table.Column<int>(type: "int", nullable: false),
                    RelayOpsPermissions = table.Column<int>(type: "int", nullable: false),
                    SmsCommandsPermissions = table.Column<int>(type: "int", nullable: false),
                    AlertPermissions = table.Column<int>(type: "int", nullable: false),
                    PanelSettingsPermissions = table.Column<int>(type: "int", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimNetworkModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessPointName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthenticationType = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimNetworkModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    UserLimit = table.Column<int>(type: "int", nullable: false),
                    SubscriptionExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredMode = table.Column<int>(type: "int", nullable: false),
                    MultiFactor = table.Column<bool>(type: "bit", nullable: false),
                    RoleManagement = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountModels_RoleModels_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RoleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Serial = table.Column<long>(type: "bigint", nullable: false),
                    SalesOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmwareVersion = table.Column<long>(type: "bigint", nullable: false),
                    WarrantyExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstalledBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteGrouping = table.Column<bool>(type: "bit", nullable: false),
                    HubAndSatellite = table.Column<bool>(type: "bit", nullable: false),
                    CallPointLimit = table.Column<int>(type: "int", nullable: false),
                    IncomingCallLimit = table.Column<int>(type: "int", nullable: false),
                    RelayOpsLimit = table.Column<int>(type: "int", nullable: false),
                    SimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceModels_SimNetworkModels_SimId",
                        column: x => x.SimId,
                        principalTable: "SimNetworkModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredMode = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MultiFactor = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    UserTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AccountModels_RoleId",
                table: "AccountModels",
                column: "RoleId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceModels");

            migrationBuilder.DropTable(
                name: "DeviceUpdateSettingsModels");

            migrationBuilder.DropTable(
                name: "OAuthTokens");

            migrationBuilder.DropTable(
                name: "UserModels");

            migrationBuilder.DropTable(
                name: "SimNetworkModels");

            migrationBuilder.DropTable(
                name: "AccountModels");

            migrationBuilder.DropTable(
                name: "RoleModels");
        }
    }
}
