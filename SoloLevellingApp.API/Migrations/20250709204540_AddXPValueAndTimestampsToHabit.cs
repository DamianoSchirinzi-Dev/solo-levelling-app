using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoloLevellingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddXPValueAndTimestampsToHabit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XPValue",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPValue",
                table: "Habits");
        }
    }
}
