using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HJ.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstallationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Installations_AppId",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "CurrentVersion",
                table: "Installations");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "TelemetryEvents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "TelemetryEvents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Operations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Operations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Operations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Installations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Installations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Installations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVersionId",
                table: "Installations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Installations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ScreenResolution",
                table: "InstallationEnvironments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OSVersion",
                table: "InstallationEnvironments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HardwareIdentifier",
                table: "InstallationEnvironments",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CpuName",
                table: "InstallationEnvironments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ApplicationLogs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ApplicationLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationEnvironments_InstallationId",
                table: "InstallationEnvironments",
                column: "InstallationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationEnvironments_Installations_InstallationId",
                table: "InstallationEnvironments",
                column: "InstallationId",
                principalTable: "Installations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationEnvironments_Installations_InstallationId",
                table: "InstallationEnvironments");

            migrationBuilder.DropIndex(
                name: "IX_InstallationEnvironments_InstallationId",
                table: "InstallationEnvironments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "TelemetryEvents");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "TelemetryEvents");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "ProductVersionId",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ApplicationLogs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ApplicationLogs");

            migrationBuilder.AddColumn<string>(
                name: "AppId",
                table: "Installations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentVersion",
                table: "Installations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ScreenResolution",
                table: "InstallationEnvironments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OSVersion",
                table: "InstallationEnvironments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HardwareIdentifier",
                table: "InstallationEnvironments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CpuName",
                table: "InstallationEnvironments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Installations_AppId",
                table: "Installations",
                column: "AppId");
        }
    }
}
