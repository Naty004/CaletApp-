using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropColumn(
                name: "MontoGasto",
                table: "Gastos");

            migrationBuilder.RenameColumn(
                name: "FechaGasto",
                table: "Gastos",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "DescripcionGasto",
                table: "Gastos",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "PorcentajeCategoria",
                table: "Categorias",
                newName: "PorcentajeMaximoMensual");

            migrationBuilder.RenameColumn(
                name: "NombreCategoria",
                table: "Categorias",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "DescripcionCategoria",
                table: "Categorias",
                newName: "Nombre");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Gastos",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Gastos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Monto",
                table: "Gastos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Categorias",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Categorias",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Categorias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Monto = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicationUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingresos_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_ApplicationUserId",
                table: "Gastos",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_ApplicationUserId",
                table: "Categorias",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_ApplicationUserId",
                table: "Ingresos",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_AspNetUsers_ApplicationUserId",
                table: "Categorias",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_AspNetUsers_ApplicationUserId",
                table: "Gastos",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_AspNetUsers_ApplicationUserId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_AspNetUsers_ApplicationUserId",
                table: "Gastos");

            migrationBuilder.DropTable(
                name: "Ingresos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_ApplicationUserId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_ApplicationUserId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Monto",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Gastos",
                newName: "FechaGasto");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Gastos",
                newName: "DescripcionGasto");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Categorias",
                newName: "NombreCategoria");

            migrationBuilder.RenameColumn(
                name: "PorcentajeMaximoMensual",
                table: "Categorias",
                newName: "PorcentajeCategoria");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Categorias",
                newName: "DescripcionCategoria");

            migrationBuilder.AddColumn<float>(
                name: "MontoGasto",
                table: "Gastos",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Apellidos = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contrasena = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CorreoElectronico = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreUsuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombres = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
