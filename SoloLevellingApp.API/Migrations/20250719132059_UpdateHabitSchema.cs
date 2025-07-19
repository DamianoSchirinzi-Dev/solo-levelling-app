using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoloLevellingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHabitSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XPValue",
                table: "Habits",
                newName: "XpValue");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Habits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ReminderEnabled",
                table: "Habits",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReminderTime",
                table: "Habits",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "ReminderEnabled",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Habits");

            migrationBuilder.RenameColumn(
                name: "XpValue",
                table: "Habits",
                newName: "XPValue");
        }
    }
}
