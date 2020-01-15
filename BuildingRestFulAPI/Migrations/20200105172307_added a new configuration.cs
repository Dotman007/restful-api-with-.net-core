using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingRestFulAPI.Migrations
{
    public partial class addedanewconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountCategories",
                columns: table => new
                {
                    AccountCategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCategories", x => x.AccountCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.AgentId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    gender = table.Column<string>(maxLength: 50, nullable: false),
                    firstname = table.Column<string>(maxLength: 50, nullable: false),
                    lastname = table.Column<string>(maxLength: 50, nullable: false),
                    CustomerUserName = table.Column<string>(nullable: true),
                    dob = table.Column<DateTime>(type: "datetime", nullable: false),
                    email = table.Column<string>(maxLength: 110, nullable: false),
                    AgentName = table.Column<string>(nullable: true),
                    AgentBank = table.Column<string>(nullable: true),
                    AgentUserName = table.Column<string>(nullable: true),
                    mainaddressid = table.Column<Guid>(nullable: false),
                    telephone = table.Column<string>(maxLength: 50, nullable: false),
                    fax = table.Column<string>(maxLength: 50, nullable: false),
                    IsAgent = table.Column<bool>(nullable: false),
                    IsCustomer = table.Column<bool>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    newsletteropted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagementRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuName = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    SourceAccountNo = table.Column<string>(nullable: true),
                    SourceAccountName = table.Column<string>(nullable: true),
                    DestinationAccountNo = table.Column<string>(nullable: true),
                    DestinationAccountName = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal", nullable: false),
                    Charge = table.Column<decimal>(type: "decimal", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal", nullable: false),
                    TransactionReference = table.Column<string>(nullable: true),
                    TransactionStatus = table.Column<string>(nullable: true),
                    IsSuccessful = table.Column<bool>(nullable: false),
                    IsFalied = table.Column<bool>(nullable: false),
                    SourceAccountType = table.Column<string>(nullable: true),
                    DestinationAccountType = table.Column<string>(nullable: true),
                    SourceBankName = table.Column<string>(nullable: true),
                    DestinationBankName = table.Column<string>(nullable: true),
                    SenderCustomerId = table.Column<Guid>(nullable: true),
                    RecieverCustomerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "AccountTransactions",
                columns: table => new
                {
                    AccountTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountNo = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(type: "decimal", nullable: false),
                    Pin = table.Column<string>(nullable: true),
                    AccountCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransactions", x => x.AccountTransactionId);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_AccountCategories_AccountCategoryId",
                        column: x => x.AccountCategoryId,
                        principalTable: "AccountCategories",
                        principalColumn: "AccountCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<Guid>(nullable: false),
                    BankName = table.Column<string>(maxLength: 15, nullable: false),
                    SortCode = table.Column<string>(maxLength: 5, nullable: false),
                    AccountNumberPrefix = table.Column<string>(maxLength: 3, nullable: false),
                    AgentName = table.Column<string>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_Banks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Managements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managements_ManagementRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ManagementRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleToMenus",
                columns: table => new
                {
                    RoleToMenuId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleToMenus", x => x.RoleToMenuId);
                    table.ForeignKey(
                        name: "FK_RoleToMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleToMenus_ManagementRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ManagementRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    AccountCategoryId = table.Column<Guid>(nullable: true),
                    AccountBalance = table.Column<decimal>(type: "decimal", nullable: false),
                    BankId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: true),
                    AccountStatus = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    AccountNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountCategories_AccountCategoryId",
                        column: x => x.AccountCategoryId,
                        principalTable: "AccountCategories",
                        principalColumn: "AccountCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountCategoryId",
                table: "Accounts",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BankId",
                table: "Accounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerId",
                table: "Accounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_AccountCategoryId",
                table: "AccountTransactions",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CustomerId",
                table: "Banks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Managements_RoleId",
                table: "Managements",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleToMenus_MenuId",
                table: "RoleToMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleToMenus_RoleId",
                table: "RoleToMenus",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountTransactions");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Managements");

            migrationBuilder.DropTable(
                name: "RoleToMenus");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "AccountCategories");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "ManagementRoles");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
