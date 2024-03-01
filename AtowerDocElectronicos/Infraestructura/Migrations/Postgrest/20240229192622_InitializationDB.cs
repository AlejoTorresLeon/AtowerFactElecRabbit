using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AtowerDocElectronico.Infraestructura.Migrations.Postgrest
{
    /// <inheritdoc />
    public partial class InitializationDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Configuracion");

            migrationBuilder.EnsureSchema(
                name: "Envios");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificacion = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    Bloqueo = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalSchema: "Configuracion",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compañia",
                schema: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DigitoVerificacion = table.Column<string>(type: "text", nullable: false),
                    Identificacion = table.Column<string>(type: "text", nullable: false),
                    RazonSocial = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IdUsuarioNubex = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IdCompanyNubex = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IdUsuarioCliente = table.Column<int>(type: "integer", nullable: false),
                    TokenNubex = table.Column<string>(type: "text", nullable: false),
                    Habilitado = table.Column<bool>(type: "boolean", nullable: false),
                    Bloqueo = table.Column<bool>(type: "boolean", nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compañia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compañia_Usuarios_IdUsuarioCliente",
                        column: x => x.IdUsuarioCliente,
                        principalSchema: "Configuracion",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                schema: "Envios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompany = table.Column<int>(type: "integer", nullable: false),
                    Prefijo = table.Column<string>(type: "text", nullable: false),
                    NumeroFactura = table.Column<string>(type: "text", nullable: false),
                    Factura = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: true),
                    FechaFactura = table.Column<string>(type: "text", nullable: true),
                    NitEmpresa = table.Column<string>(type: "text", nullable: true),
                    DocumentoAdquisiente = table.Column<string>(type: "text", nullable: true),
                    ValorFactura = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorIva = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorOtro = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: true),
                    Cufe = table.Column<string>(type: "text", nullable: true),
                    Contrato = table.Column<string>(type: "text", nullable: true),
                    DireccionFacturaDian = table.Column<string>(type: "text", nullable: true),
                    Base64Pdf = table.Column<byte[]>(type: "bytea", nullable: true),
                    JsonEnvioAtower = table.Column<string>(type: "text", nullable: true),
                    JsonEnvioNubex = table.Column<string>(type: "text", nullable: true),
                    JsonRespuestaNubex = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturas_Compañia_IdCompany",
                        column: x => x.IdCompany,
                        principalSchema: "Configuracion",
                        principalTable: "Compañia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Configuracion",
                table: "Roles",
                columns: new[] { "Id", "Codigo", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Dev", "Developers" },
                    { 2, "Admin", "Administrador" },
                    { 3, "Supp", "Soporte" },
                    { 4, "Cli", "Cliente" }
                });

            migrationBuilder.InsertData(
                schema: "Configuracion",
                table: "Usuarios",
                columns: new[] { "Id", "Bloqueo", "Email", "IdRol", "Identificacion", "Nombre", "PasswordHash", "PasswordSalt" },
                values: new object[,]
                {
                    { 1, false, "support@atowers.com.co", 1, 0m, "Developer Atower", "wvjq33+Fr8kCV1jZ9zTVaNIjiuWRI42H4FRRzuslajjZAlKJsCoDz9MxAz8X8ZigLEyVXImy24PgpQvC1vvlxg==", "Tr8QpOR+IGweNWsKLLwVnv8kEFAOxwxmFUNRlRHN3KxvaEU7V5mxpnWPfFXWW5hIWelGrVErjer7UyucfCKA8A==" },
                    { 2, false, "admin@gmail.com", 2, 123456789m, "Admin", "INOApmD86A+GSlT35x9CBhrkSgVpgX3TL3Ybz7YEBRrsKX2ova2sWcRmqOsEuZFrxEhGluZMcgdAeFuaOGA2JQ==", "b9BTJgJ1SeGjLn+EUD4mIbUURLdeIL2TqG5572ZM3ommw5MbJ5ebZqgBXovi5i+M3S82B6jyfqY4upBeHEslhg==" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compañia_IdUsuarioCliente",
                schema: "Configuracion",
                table: "Compañia",
                column: "IdUsuarioCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdCompany",
                schema: "Envios",
                table: "Facturas",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                schema: "Configuracion",
                table: "Usuarios",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas",
                schema: "Envios");

            migrationBuilder.DropTable(
                name: "Compañia",
                schema: "Configuracion");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "Configuracion");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Configuracion");
        }
    }
}
