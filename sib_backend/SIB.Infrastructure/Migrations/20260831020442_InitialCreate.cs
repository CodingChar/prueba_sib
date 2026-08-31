using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SB.Management.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimerNombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroSeguroSocial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoAsalariado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SalarioSemanal = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoAsalariado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadoAsalariado_Empleado_Id",
                        column: x => x.Id,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoAsalariadoComision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    VentasBrutas = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    TarifaComision = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    SalarioBase = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoAsalariadoComision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadoAsalariadoComision_Empleado_Id",
                        column: x => x.Id,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoPorComision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    VentasBrutas = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    TarifaComision = table.Column<decimal>(type: "decimal(6,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoPorComision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadoPorComision_Empleado_Id",
                        column: x => x.Id,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoPorHora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SueldoPorHora = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoPorHora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadoPorHora_Empleado_Id",
                        column: x => x.Id,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    FechaPago = table.Column<DateOnly>(type: "date", nullable: false),
                    MontoCalculado = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    DetalleCalculo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Empleado_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_NumeroSeguroSocial",
                table: "Empleado",
                column: "NumeroSeguroSocial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_EmpleadoId",
                table: "Pagos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpleadoAsalariado");

            migrationBuilder.DropTable(
                name: "EmpleadoAsalariadoComision");

            migrationBuilder.DropTable(
                name: "EmpleadoPorComision");

            migrationBuilder.DropTable(
                name: "EmpleadoPorHora");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
