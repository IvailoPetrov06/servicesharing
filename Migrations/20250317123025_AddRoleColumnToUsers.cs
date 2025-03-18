using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace servicesharing.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleColumnToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                nullable: true);
        }
    }
}
