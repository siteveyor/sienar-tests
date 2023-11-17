using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Sienar;

#nullable disable

namespace Project.Data.Migrations
{
	/// <inheritdoc />
	public partial class Initial : Migration
	{
		private string _adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
		private string _adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
		private string _adminPasswordHash = Environment.GetEnvironmentVariable("ADMIN_PASSWORD_HASH");
		private string _adminUserId = Environment.GetEnvironmentVariable("ADMIN_USER_ID");
		private string _adminRoleId = Environment.GetEnvironmentVariable("ADMIN_ROLE_ID");

		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterDatabase()
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					Name = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					ConcurrencyStamp = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => x.Id);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "States",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Abbreviation = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					ConcurrencyStamp = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_States", x => x.Id);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					ConcurrencyStamp = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					Username = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					PasswordHash = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					LoginFailedCount = table.Column<int>(type: "int", nullable: false),
					LockoutEnd = table.Column<DateTime>(type: "datetime(6)", nullable: true),
					TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
					Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
					PendingEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "AppUserSienarRole",
				columns: table => new
				{
					AppUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					RolesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserSienarRole", x => new { x.AppUserId, x.RolesId });
					table.ForeignKey(
						name: "FK_AppUserSienarRole_Roles_RolesId",
						column: x => x.RolesId,
						principalTable: "Roles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AppUserSienarRole_Users_AppUserId",
						column: x => x.AppUserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "VerificationCode",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					Code = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					Type = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
					AppUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
					ConcurrencyStamp = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_VerificationCode", x => x.Id);
					table.ForeignKey(
						name: "FK_VerificationCode_Users_AppUserId",
						column: x => x.AppUserId,
						principalTable: "Users",
						principalColumn: "Id");
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateIndex(
				name: "IX_AppUserSienarRole_RolesId",
				table: "AppUserSienarRole",
				column: "RolesId");

			migrationBuilder.CreateIndex(
				name: "IX_States_Abbreviation",
				table: "States",
				column: "Abbreviation",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_States_Name",
				table: "States",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Users_Email",
				table: "Users",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Users_Username",
				table: "Users",
				column: "Username",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_VerificationCode_AppUserId",
				table: "VerificationCode",
				column: "AppUserId");

			// Add admin user account
			migrationBuilder.Sql($"INSERT INTO Users (Id, ConcurrencyStamp, Username, PasswordHash, LoginFailedCount, LockoutEnd, TwoFactorEnabled, Email, EmailConfirmed, PendingEmail, PhoneNumber, PhoneNumberConfirmed) VALUES ('{_adminUserId}', uuid(), '{_adminUsername}', '{_adminPasswordHash}', 0, null, 0, '{_adminEmail}', 1, null, null, 0)");

			// Set up roles
			migrationBuilder.Sql($"INSERT INTO Roles (Id, Name, ConcurrencyStamp) VALUES ('{_adminRoleId}', '{Roles.Admin}', uuid())");

			// Map roles
			migrationBuilder.Sql($"INSERT INTO AppUserSienarRole (AppUserId, RolesId) VALUES ('{_adminUserId}', '{_adminRoleId}')");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AppUserSienarRole");

			migrationBuilder.DropTable(
				name: "States");

			migrationBuilder.DropTable(
				name: "VerificationCode");

			migrationBuilder.DropTable(
				name: "Roles");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
