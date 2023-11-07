using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class addTicketPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "SeatBooking");

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "ShowTime",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "ShowTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "SeatBooking",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
