﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectroKart.Common.Migrations
{
    /// <inheritdoc />
    public partial class addednewcancelledDatecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "Orders");
        }
    }
}
