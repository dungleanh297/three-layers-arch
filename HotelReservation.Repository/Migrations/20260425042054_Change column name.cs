using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelReservation.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Changecolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Rooms",
                newName: "HasBeenTaken");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "HasBeenTaken",
                table: "Rooms",
                newName: "IsAvailable");
        }
    }
}
