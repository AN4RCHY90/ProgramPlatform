using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLockoutEndColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a new column with the type nvarchar(max) to hold the LockoutEnd values as text temporarily
            migrationBuilder.AddColumn<string>(
                name: "TempLockoutEnd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            // Step 2: Copy data from old column to new column
            migrationBuilder.Sql("UPDATE AspNetUsers SET TempLockoutEnd = LockoutEnd");

            // Step 3: Drop the old LockoutEnd column
            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AspNetUsers");

            // Step 4: Add the new LockoutEnd column with the correct type
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            // Step 5: Copy data back from the TempLockoutEnd column to the new LockoutEnd column after converting
            migrationBuilder.Sql("UPDATE AspNetUsers SET LockoutEnd = TRY_CAST(TempLockoutEnd AS datetimeoffset)");

            // Step 6: Drop the temporary column
            migrationBuilder.DropColumn(
                name: "TempLockoutEnd",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add the old column back with the original type
            migrationBuilder.AddColumn<string>(
                name: "OldLockoutEnd",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            // Step 2: Copy data back to the old column
            migrationBuilder.Sql("UPDATE AspNetUsers SET OldLockoutEnd = CAST(LockoutEnd AS text)");

            // Step 3: Drop the new column
            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AspNetUsers");

            // Step 4: Rename the old column back to the original name
            migrationBuilder.RenameColumn(
                name: "OldLockoutEnd",
                table: "AspNetUsers",
                newName: "LockoutEnd");
        }
    }
}
