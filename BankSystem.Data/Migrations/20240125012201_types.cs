using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_SenderId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfers");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Transfers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Transfers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId1",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_BankAccountId",
                table: "Transfers",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_BankAccountId1",
                table: "Transfers",
                column: "BankAccountId1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BankAccounts_Balance",
                table: "BankAccounts",
                sql: "[AccountBalance] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_BankAccountId",
                table: "Transfers",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_BankAccountId1",
                table: "Transfers",
                column: "BankAccountId1",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_BankAccountId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_BankAccountId1",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_BankAccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_BankAccountId1",
                table: "Transfers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BankAccounts_Balance",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "BankAccountId1",
                table: "Transfers");

            migrationBuilder.AlterColumn<Guid>(
                name: "SenderId",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReceiverId",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ReceiverId",
                table: "Transfers",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfers",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_ReceiverId",
                table: "Transfers",
                column: "ReceiverId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }
    }
}
