using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomColumnsToAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE AspNetUsers
                ADD FirstName NVARCHAR(256) NOT NULL DEFAULT '',
                    LastName NVARCHAR(256) NOT NULL DEFAULT '',
                    PreferredMode INT NOT NULL DEFAULT 0,
                    MultiFactor BIT NOT NULL DEFAULT 0,
                    IsAdmin BIT NOT NULL DEFAULT 0,
                    UserTypes NVARCHAR(MAX) NULL,
                    IsArchived BIT NOT NULL DEFAULT 0;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE AspNetUsers
                DROP COLUMN FirstName,
                    LastName,
                    PreferredMode,
                    MultiFactor,
                    IsAdmin,
                    UserTypes,
                    AccountId,
                    IsArchived;
            ");
            
            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
