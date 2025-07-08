using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoloLevellingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingHabitPropertiesAndFixXPLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "XPLogs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCompletedAt",
                table: "Habits",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_XPLogs_HabitCompletionId",
                table: "XPLogs",
                column: "HabitCompletionId");

            migrationBuilder.CreateIndex(
                name: "IX_XPLogs_Timestamp",
                table: "XPLogs",
                column: "Timestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_XPLogs_HabitCompletions_HabitCompletionId",
                table: "XPLogs",
                column: "HabitCompletionId",
                principalTable: "HabitCompletions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XPLogs_HabitCompletions_HabitCompletionId",
                table: "XPLogs");

            migrationBuilder.DropIndex(
                name: "IX_XPLogs_HabitCompletionId",
                table: "XPLogs");

            migrationBuilder.DropIndex(
                name: "IX_XPLogs_Timestamp",
                table: "XPLogs");

            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "LastCompletedAt",
                table: "Habits");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "XPLogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
