using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.GlobalDBMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
                    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
                ) CHARACTER SET=utf8mb4;

                ALTER DATABASE CHARACTER SET utf8mb4;

                CREATE TABLE `account_info` (
                    `AccountNo` bigint NOT NULL AUTO_INCREMENT,
                    `MemeberId` longtext CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK_account_info` PRIMARY KEY (`AccountNo`)
                ) CHARACTER SET=utf8mb4;
                
                """);

            // INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES ('20250629051340_Initial', '9.0.6');
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_info");
        }
    }
}
