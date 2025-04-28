using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketBookingRelationshipToBookedSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketBookingId",
                table: "BookedSeats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_TicketBookingId",
                table: "BookedSeats",
                column: "TicketBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedSeats_TicketBookings_TicketBookingId",
                table: "BookedSeats",
                column: "TicketBookingId",
                principalTable: "TicketBookings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedSeats_TicketBookings_TicketBookingId",
                table: "BookedSeats");

            migrationBuilder.DropIndex(
                name: "IX_BookedSeats_TicketBookingId",
                table: "BookedSeats");

            migrationBuilder.DropColumn(
                name: "TicketBookingId",
                table: "BookedSeats");
        }
    }
}
