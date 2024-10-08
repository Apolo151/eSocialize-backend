﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaApp.Migrations
{
    /// <inheritdoc />
    public partial class addFollowings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Authors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorId",
                table: "Authors",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Authors_AuthorId",
                table: "Authors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Authors_AuthorId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Authors");
        }
    }
}
