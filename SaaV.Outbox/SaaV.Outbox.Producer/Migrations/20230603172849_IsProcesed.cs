using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaV.Outbox.Producer.Migrations
{
    /// <inheritdoc />
    public partial class IsProcesed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "OutboxMessage",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "OutboxMessage");
        }
    }
}
