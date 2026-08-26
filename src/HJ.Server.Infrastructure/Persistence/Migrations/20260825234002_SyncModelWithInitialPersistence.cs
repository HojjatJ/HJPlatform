using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HJ.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelWithInitialPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationExecution_Operations_OperationId",
                table: "OperationExecution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperationExecution",
                table: "OperationExecution");

            migrationBuilder.RenameTable(
                name: "OperationExecution",
                newName: "OperationExecutions");

            migrationBuilder.RenameIndex(
                name: "IX_OperationExecution_OperationId",
                table: "OperationExecutions",
                newName: "IX_OperationExecutions_OperationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperationExecutions",
                table: "OperationExecutions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationExecutions_Operations_OperationId",
                table: "OperationExecutions",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationExecutions_Operations_OperationId",
                table: "OperationExecutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperationExecutions",
                table: "OperationExecutions");

            migrationBuilder.RenameTable(
                name: "OperationExecutions",
                newName: "OperationExecution");

            migrationBuilder.RenameIndex(
                name: "IX_OperationExecutions_OperationId",
                table: "OperationExecution",
                newName: "IX_OperationExecution_OperationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperationExecution",
                table: "OperationExecution",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationExecution_Operations_OperationId",
                table: "OperationExecution",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
