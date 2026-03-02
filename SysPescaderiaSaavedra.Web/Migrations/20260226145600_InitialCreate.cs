using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SysPescaderiaSaavedra.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    categoria_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__DB875A4FA143C463", x => x.categoria_id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    cliente_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    identificacion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__47E34D64790CAAC5", x => x.cliente_id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    proveedor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_empresa = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    contacto = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    nit = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__88BBADA49BA4A831", x => x.proveedor_id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__CF32E4435D492887", x => x.rol_id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    producto_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoria_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    unidad_medida = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "kg"),
                    precio_venta = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    stock_global = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    publicado_web = table.Column<bool>(type: "bit", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__FB5CEEECBA6DD7ED", x => x.producto_id);
                    table.ForeignKey(
                        name: "FK__Productos__categ__5812160E",
                        column: x => x.categoria_id,
                        principalTable: "Categorias",
                        principalColumn: "categoria_id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    nombre_usuario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    clave_hash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    nombre_completo = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__2ED7D2AF796838DD", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK__Usuarios__rol_id__44FF419A",
                        column: x => x.rol_id,
                        principalTable: "Roles",
                        principalColumn: "rol_id");
                });

            migrationBuilder.CreateTable(
                name: "Ingreso_Mercaderia",
                columns: table => new
                {
                    ingreso_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    proveedor_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    total_compra = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ingreso___4E42CFD9027D0CC7", x => x.ingreso_id);
                    table.ForeignKey(
                        name: "FK__Ingreso_M__prove__5BE2A6F2",
                        column: x => x.proveedor_id,
                        principalTable: "Proveedores",
                        principalColumn: "proveedor_id");
                    table.ForeignKey(
                        name: "FK__Ingreso_M__usuar__5CD6CB2B",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id");
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    venta_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    fecha_venta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    impuesto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas__B1350809C61DF778", x => x.venta_id);
                    table.ForeignKey(
                        name: "FK__Ventas__cliente___6754599E",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "cliente_id");
                    table.ForeignKey(
                        name: "FK__Ventas__usuario___68487DD7",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id");
                });

            migrationBuilder.CreateTable(
                name: "Lotes",
                columns: table => new
                {
                    lote_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ingreso_id = table.Column<int>(type: "int", nullable: false),
                    producto_id = table.Column<int>(type: "int", nullable: false),
                    codigo_lote = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    cantidad_inicial = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    stock_actual = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    costo_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_produccion = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lotes__E0E20FB3F37BC898", x => x.lote_id);
                    table.ForeignKey(
                        name: "FK__Lotes__ingreso_i__60A75C0F",
                        column: x => x.ingreso_id,
                        principalTable: "Ingreso_Mercaderia",
                        principalColumn: "ingreso_id");
                    table.ForeignKey(
                        name: "FK__Lotes__producto___619B8048",
                        column: x => x.producto_id,
                        principalTable: "Productos",
                        principalColumn: "producto_id");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Venta",
                columns: table => new
                {
                    detalle_venta_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    venta_id = table.Column<int>(type: "int", nullable: false),
                    producto_id = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    precio_venta_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___652699E323D89012", x => x.detalle_venta_id);
                    table.ForeignKey(
                        name: "FK__Detalle_V__produ__6C190EBB",
                        column: x => x.producto_id,
                        principalTable: "Productos",
                        principalColumn: "producto_id");
                    table.ForeignKey(
                        name: "FK__Detalle_V__venta__6B24EA82",
                        column: x => x.venta_id,
                        principalTable: "Ventas",
                        principalColumn: "venta_id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Categori__72AFBCC6449EBDC1",
                table: "Categorias",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Clientes__AB6E6164E918D209",
                table: "Clientes",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Clientes__C196DEC70B02F626",
                table: "Clientes",
                column: "identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Venta_producto_id",
                table: "Detalle_Venta",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Venta_venta_id",
                table: "Detalle_Venta",
                column: "venta_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ingreso_Mercaderia_proveedor_id",
                table: "Ingreso_Mercaderia",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ingreso_Mercaderia_usuario_id",
                table: "Ingreso_Mercaderia",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_ingreso_id",
                table: "Lotes",
                column: "ingreso_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_producto_id",
                table: "Lotes",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_categoria_id",
                table: "Productos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Proveedo__AB6E616474F21045",
                table: "Proveedores",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Proveedo__DF97D0E42AAA13B8",
                table: "Proveedores",
                column: "nit",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Roles__72AFBCC661415CBB",
                table: "Roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_rol_id",
                table: "Usuarios",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__AB6E6164BA632CFF",
                table: "Usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__D4D22D74B609B67F",
                table: "Usuarios",
                column: "nombre_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_cliente_id",
                table: "Ventas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_usuario_id",
                table: "Ventas",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalle_Venta");

            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Ingreso_Mercaderia");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
