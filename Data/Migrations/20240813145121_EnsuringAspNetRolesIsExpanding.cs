using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnsuringAspNetRolesIsExpanding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE AspNetRoles
                ADD UserType INT NOT NULL DEFAULT 0,
                    PullDeviceProgramming INT NOT NULL DEFAULT 0,
                    ApiAccess BIT NOT NULL DEFAULT 0,
                    DashboardAccess BIT NOT NULL DEFAULT 0,
                    ProgrammingRollBack BIT NOT NULL DEFAULT 0,
                    ActivityLogPermissions INT NOT NULL DEFAULT 0,
                    CallPointPermissions INT NOT NULL DEFAULT 0,
                IncomingCallPermissions INT NOT NULL DEFAULT 0,
                TimePeriodPermissions INT NOT NULL DEFAULT 0,
                RelayOpsPermissions INT NOT NULL DEFAULT 0,
                SmsCommandsPermissions INT NOT NULL DEFAULT 0,
                AlertPermissions INT NOT NULL DEFAULT 0,
                PanelSettingsPermissions INT NOT NULL DEFAULT 0,
                IsArchived BIT NOT NULL DEFAULT 0;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE AspNetRoles
                DROP COLUMN UserType,
                    PullDeviceProgramming,
                    ApiAccess,
                    DashboardAccess,
                    ProgrammingRollBack,
                    ActivityLogPermissions,
                    CallPointPermissions,
                    IncomingCallPermissions,
                    TimePeriodPermissions,
                    RelayOpsPermissions,
                    SmsCommandsPermissions,
                    AlertPermissions,
                    PanelSettingsPermissions,
                    IsArchived;
            ");
        }
    }
}
