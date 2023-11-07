using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class addbookingStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_ShowTime_ShowTimeId",
                table: "Seats");

            migrationBuilder.AddColumn<int>(
                name: "BookingStatus",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_ShowTime_ShowTimeId",
                table: "Seats",
                column: "ShowTimeId",
                principalTable: "ShowTime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_ShowTime_ShowTimeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_ShowTime_ShowTimeId",
                table: "Seats",
                column: "ShowTimeId",
                principalTable: "ShowTime",
                principalColumn: "Id");
        }
    }
}
