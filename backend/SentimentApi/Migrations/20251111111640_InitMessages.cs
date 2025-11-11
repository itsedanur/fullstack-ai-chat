using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SentimentApi.Migrations
{
    /// <inheritdoc />
    public partial class InitMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Sentiment",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nick",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nick",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Sentiment",
                table: "Messages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<double>(
                name: "Confidence",
                table: "Messages",
                type: "REAL",
                nullable: true);
        }
    }
}
