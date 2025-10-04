using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_unit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "province_city",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_province_city", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(55)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    product_unit_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_product_unit_product_unit_id",
                        column: x => x.product_unit_id,
                        principalTable: "product_unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "commune_ward",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ProvinceCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commune_ward", x => x.code);
                    table.ForeignKey(
                        name: "FK_commune_ward_province_city_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "province_city",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_batch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_batch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_batch_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    cid = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    specific_address = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    commune_ward_code = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_commune_ward_commune_ward_code",
                        column: x => x.commune_ward_code,
                        principalTable: "commune_ward",
                        principalColumn: "code",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_users_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commune_ward_name",
                table: "commune_ward",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_commune_ward_ProvinceCode",
                table: "commune_ward",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_product_name",
                table: "product",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_product_product_unit_id",
                table: "product",
                column: "product_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_batch_product_id",
                table: "product_batch",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_province_city_name",
                table: "province_city",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_role_name",
                table: "role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_commune_ward_code",
                table: "users",
                column: "commune_ward_code");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_batch");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "commune_ward");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "product_unit");

            migrationBuilder.DropTable(
                name: "province_city");
        }
    }
}
