using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HJ.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstallationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    ExceptionJson = table.Column<string>(type: "text", nullable: true),
                    PropertiesJson = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstallationEnvironments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstallationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OSVersion = table.Column<string>(type: "text", nullable: true),
                    CpuName = table.Column<string>(type: "text", nullable: true),
                    CpuCoreCount = table.Column<int>(type: "integer", nullable: false),
                    RamGB = table.Column<double>(type: "double precision", nullable: false),
                    ScreenResolution = table.Column<string>(type: "text", nullable: true),
                    HardwareIdentifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationEnvironments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Installations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstallationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstallationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingJobes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ExecutionSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FilesCount = table.Column<int>(type: "integer", nullable: false),
                    SuccessCount = table.Column<int>(type: "integer", nullable: false),
                    FailedCount = table.Column<int>(type: "integer", nullable: false),
                    TargetSizeKB = table.Column<long>(type: "bigint", nullable: false),
                    SavedBytes = table.Column<long>(type: "bigint", nullable: false),
                    DurationMs = table.Column<long>(type: "bigint", nullable: false),
                    ProcessingMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ConcurrencyLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingJobes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstallationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    EventVersion = table.Column<int>(type: "integer", nullable: false),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLogs_CreatedAt",
                table: "ApplicationLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLogs_InstallationId",
                table: "ApplicationLogs",
                column: "InstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLogs_Level",
                table: "ApplicationLogs",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLogs_OperationId",
                table: "ApplicationLogs",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_AppId",
                table: "Installations",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_InstallationId",
                table: "Installations",
                column: "InstallationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CorrelationId",
                table: "Operations",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_InstallationId",
                table: "Operations",
                column: "InstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_StartedAt",
                table: "Operations",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Type",
                table: "Operations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingJobes_BatchId",
                table: "ProcessingJobes",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingJobes_OperationId",
                table: "ProcessingJobes",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryEvents_EventName_CreatedAt",
                table: "TelemetryEvents",
                columns: new[] { "EventName", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryEvents_InstallationId",
                table: "TelemetryEvents",
                column: "InstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryEvents_OperationId",
                table: "TelemetryEvents",
                column: "OperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationLogs");

            migrationBuilder.DropTable(
                name: "InstallationEnvironments");

            migrationBuilder.DropTable(
                name: "Installations");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "ProcessingJobes");

            migrationBuilder.DropTable(
                name: "TelemetryEvents");
        }
    }
}

