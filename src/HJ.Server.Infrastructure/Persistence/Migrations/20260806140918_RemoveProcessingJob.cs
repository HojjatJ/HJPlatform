using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HJ.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProcessingJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessingJobes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessingJobes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConcurrencyLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationMs = table.Column<long>(type: "bigint", nullable: false),
                    ExecutionSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FailedCount = table.Column<int>(type: "integer", nullable: false),
                    FilesCount = table.Column<int>(type: "integer", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SavedBytes = table.Column<long>(type: "bigint", nullable: false),
                    SuccessCount = table.Column<int>(type: "integer", nullable: false),
                    TargetSizeKB = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingJobes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingJobes_BatchId",
                table: "ProcessingJobes",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingJobes_OperationId",
                table: "ProcessingJobes",
                column: "OperationId");
        }
    }
}
