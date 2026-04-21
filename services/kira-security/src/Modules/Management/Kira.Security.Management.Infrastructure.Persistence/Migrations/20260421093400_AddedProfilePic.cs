using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kira.Security.Management.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePic",
                schema: "kira-management",
                table: "user_profiles",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                schema: "kira-management",
                table: "user_profiles");
        }
    }
}
