using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserAppId1",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserAppId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UserAppId1",
                table: "Addresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserAppId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserAppId",
                table: "Addresses",
                column: "UserAppId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserAppId",
                table: "Addresses",
                column: "UserAppId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserAppId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserAppId",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "UserAppId",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserAppId1",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserAppId1",
                table: "Addresses",
                column: "UserAppId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserAppId1",
                table: "Addresses",
                column: "UserAppId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
