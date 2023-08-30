using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserAppId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserAppId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserAppId1",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserAppId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserAppId",
                table: "Products",
                column: "UserAppId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserAppId",
                table: "Products",
                column: "UserAppId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserAppId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserAppId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "UserAppId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserAppId1",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserAppId1",
                table: "Products",
                column: "UserAppId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserAppId1",
                table: "Products",
                column: "UserAppId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
