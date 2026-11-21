using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoZOGLEY.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin2Dto",
                columns: table => new
                {
                    Totales = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AdminDto",
                columns: table => new
                {
                    Mes = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Año = table.Column<int>(type: "int", nullable: false),
                    IngresosTotales = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CategoriaRopa",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaRopa", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "ProductosRopaDto",
                columns: table => new
                {
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EnlaceIMG = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProductosSimpleDto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    NombreProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UsuariosInfo",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreApellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Contrasenia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosInfo", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosInformacionCompletaDto",
                columns: table => new
                {
                    NombreApellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "VentasAgrupadasDto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadTotal = table.Column<int>(type: "int", nullable: false),
                    PrecioIndividual = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalGenerado = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProductosRopa",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    EnlaceIMG = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosRopa", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_ProductosRopa_CategoriaRopa",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriaRopa",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComprasUsuario",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Talla = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FechaCompra = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasUsuario", x => x.IdCompra);
                    table.ForeignKey(
                        name: "FK_ProductoCompras",
                        column: x => x.IdProducto,
                        principalTable: "ProductosRopa",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCompras",
                        column: x => x.IdUsuario,
                        principalTable: "UsuariosInfo",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventarioProductos",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioProductos", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_InventarioProductos_ProductosRopa",
                        column: x => x.IdProducto,
                        principalTable: "ProductosRopa",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    IdOferta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    DescuentoMonto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.IdOferta);
                    table.ForeignKey(
                        name: "FK_Ofertas_ProductosRopa",
                        column: x => x.IdProducto,
                        principalTable: "ProductosRopa",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComprasUsuario_IdProducto",
                table: "ComprasUsuario",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasUsuario_IdUsuario",
                table: "ComprasUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioProductos_IdProducto",
                table: "InventarioProductos",
                column: "IdProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_IdProducto",
                table: "Ofertas",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosRopa_IdCategoria",
                table: "ProductosRopa",
                column: "IdCategoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin2Dto");

            migrationBuilder.DropTable(
                name: "AdminDto");

            migrationBuilder.DropTable(
                name: "ComprasUsuario");

            migrationBuilder.DropTable(
                name: "InventarioProductos");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "ProductosRopaDto");

            migrationBuilder.DropTable(
                name: "ProductosSimpleDto");

            migrationBuilder.DropTable(
                name: "UsuariosInformacionCompletaDto");

            migrationBuilder.DropTable(
                name: "VentasAgrupadasDto");

            migrationBuilder.DropTable(
                name: "UsuariosInfo");

            migrationBuilder.DropTable(
                name: "ProductosRopa");

            migrationBuilder.DropTable(
                name: "CategoriaRopa");
        }
    }
}
