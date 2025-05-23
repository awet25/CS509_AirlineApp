﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDOBAndGenderToTicketBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "TicketBookings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "TicketBookings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "TicketBookings");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "TicketBookings");
        }
    }
}
