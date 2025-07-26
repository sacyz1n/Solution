using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.GlobalDBMigrations
{
    /// <inheritdoc />
    public partial class ModifyAccountInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
               ALTER TABLE `account_info` RENAME COLUMN `MemeberId` TO `MemberId`;
               ALTER TABLE `account_info` ADD `PlatformType` tinyint unsigned NOT NULL DEFAULT 0 AFTER `MemberId`;
               ALTER TABLE `account_info` ADD `LoginTime` DATETIME NOT NULL DEFAULT '0001-01-01 00:00:00' AFTER `PlatformType`;
               ALTER TABLE `account_info` ADD `CreateTime` DATETIME NOT NULL DEFAULT '0001-01-01 00:00:00' AFTER `LoginTime`;
               """);

            // INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES ('20250726032025_ModifyAccountInfoTable', '9.0.6');

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "account_info");

            migrationBuilder.DropColumn(
                name: "LoginTime",
                table: "account_info");

            migrationBuilder.DropColumn(
                name: "PlatformType",
                table: "account_info");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "account_info",
                newName: "MemeberId");
        }
    }
}
