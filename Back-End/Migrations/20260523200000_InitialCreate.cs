using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260523200000_InitialCreate")]
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ONG",
                columns: table => new
                {
                    ID_ONG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DSC_NOME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NUM_CNPJ = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    NUM_TELEFONE = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    DSC_EMAIL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DSC_ENDERECO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONG", x => x.ID_ONG);
                });

            migrationBuilder.CreateTable(
                name: "STATUS",
                columns: table => new
                {
                    ID_STATUS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DSC_STATUS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATUS", x => x.ID_STATUS);
                });

            migrationBuilder.CreateTable(
                name: "UF",
                columns: table => new
                {
                    ID_UF = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIGLA_UF = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DSC_NOME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UF", x => x.ID_UF);
                });

            migrationBuilder.CreateTable(
                name: "ADMIN",
                columns: table => new
                {
                    ID_ADMIN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_ONG = table.Column<int>(type: "int", nullable: false),
                    DSC_SENHA_HASH = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DSC_SENHA_SALT = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DSC_NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DSC_EMAIL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NUM_TELEFONE = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN", x => x.ID_ADMIN);
                    table.ForeignKey(
                        name: "FK_ADMIN_ONG_ID_ONG",
                        column: x => x.ID_ONG,
                        principalTable: "ONG",
                        principalColumn: "ID_ONG",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ATENDENTE",
                columns: table => new
                {
                    ID_ATENDENTE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_ONG = table.Column<int>(type: "int", nullable: false),
                    DSC_SENHA_HASH = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DSC_SENHA_SALT = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DSC_NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DSC_EMAIL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NUM_TELEFONE = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATENDENTE", x => x.ID_ATENDENTE);
                    table.ForeignKey(
                        name: "FK_ATENDENTE_ONG_ID_ONG",
                        column: x => x.ID_ONG,
                        principalTable: "ONG",
                        principalColumn: "ID_ONG",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NUM_CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DSC_ENDERECO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DSC_CIDADE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DATA_NASCIMENTO = table.Column<DateTime>(type: "date", nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DSC_TIPO_PARTICIPACAO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ID_UF = table.Column<int>(type: "int", nullable: false),
                    DSC_NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DSC_EMAIL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NUM_TELEFONE = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_USUARIO_UF_ID_UF",
                        column: x => x.ID_UF,
                        principalTable: "UF",
                        principalColumn: "ID_UF",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "INTERESSE",
                columns: table => new
                {
                    ID_INTERESSE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false),
                    DSC_TIPO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DSC_DESCRICAO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ID_STATUS = table.Column<int>(type: "int", nullable: false),
                    DATA_REGISTRO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INTERESSE", x => x.ID_INTERESSE);
                    table.ForeignKey(
                        name: "FK_INTERESSE_STATUS_ID_STATUS",
                        column: x => x.ID_STATUS,
                        principalTable: "STATUS",
                        principalColumn: "ID_STATUS",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INTERESSE_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_DSC_EMAIL",
                table: "ADMIN",
                column: "DSC_EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_ID_ONG",
                table: "ADMIN",
                column: "ID_ONG");

            migrationBuilder.CreateIndex(
                name: "IX_ATENDENTE_DSC_EMAIL",
                table: "ATENDENTE",
                column: "DSC_EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ATENDENTE_ID_ONG",
                table: "ATENDENTE",
                column: "ID_ONG");

            migrationBuilder.CreateIndex(
                name: "IX_INTERESSE_ID_STATUS",
                table: "INTERESSE",
                column: "ID_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_INTERESSE_ID_USUARIO",
                table: "INTERESSE",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_ONG_NUM_CNPJ",
                table: "ONG",
                column: "NUM_CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_STATUS_DSC_STATUS",
                table: "STATUS",
                column: "DSC_STATUS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UF_SIGLA_UF",
                table: "UF",
                column: "SIGLA_UF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ID_UF",
                table: "USUARIO",
                column: "ID_UF");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_NUM_CPF",
                table: "USUARIO",
                column: "NUM_CPF",
                unique: true);

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [ONG] WHERE [ID_ONG] = 1)
BEGIN
    SET IDENTITY_INSERT [ONG] ON;
    INSERT INTO [ONG] ([ID_ONG], [DSC_NOME], [NUM_CNPJ], [NUM_TELEFONE], [DSC_EMAIL], [DSC_ENDERECO])
    VALUES (1, N'Conecta++', N'12345678000199', N'11919100860', N'contato@ong.com', N'Rua Gugu, Sao Paulo SP');
    SET IDENTITY_INSERT [ONG] OFF;
END");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [STATUS] WHERE [DSC_STATUS] = N'Pendente')
BEGIN
    INSERT INTO [STATUS] ([DSC_STATUS]) VALUES (N'Pendente');
END
IF NOT EXISTS (SELECT 1 FROM [STATUS] WHERE [DSC_STATUS] = N'Concluído')
BEGIN
    INSERT INTO [STATUS] ([DSC_STATUS]) VALUES (N'Concluído');
END
IF NOT EXISTS (SELECT 1 FROM [STATUS] WHERE [DSC_STATUS] = N'Cancelado')
BEGIN
    INSERT INTO [STATUS] ([DSC_STATUS]) VALUES (N'Cancelado');
END");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [ADMIN] WHERE [DSC_EMAIL] = N'admin@conecta.com')
BEGIN
    SET IDENTITY_INSERT [ADMIN] ON;
    INSERT INTO [ADMIN] ([ID_ADMIN], [ID_ONG], [DSC_NOME], [DSC_EMAIL], [NUM_TELEFONE], [DSC_SENHA_HASH], [DSC_SENHA_SALT])
    VALUES (1, 1, N'Administrador', N'admin@conecta.com', N'11999999999', N'WFxOGGXQds8tge1Z2whoyAnjWybNL5cxxoZ/LsuraGY=', N'oRpZ5CPbtDjoAVfXNM1vUg==');
    SET IDENTITY_INSERT [ADMIN] OFF;
END");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [ATENDENTE] WHERE [DSC_EMAIL] = N'atendente@conecta.com')
BEGIN
    SET IDENTITY_INSERT [ATENDENTE] ON;
    INSERT INTO [ATENDENTE] ([ID_ATENDENTE], [ID_ONG], [DSC_NOME], [DSC_EMAIL], [NUM_TELEFONE], [DSC_SENHA_HASH], [DSC_SENHA_SALT])
    VALUES (1, 1, N'Atendente', N'atendente@conecta.com', N'11988888888', N'OOtdX6jwXh8TZAamGnhtMxPVjtqV864l3u8mev8t6Qw=', N'ZzsWoAWNAzhxJTonpQoz+Q==');
    SET IDENTITY_INSERT [ATENDENTE] OFF;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ADMIN");
            migrationBuilder.DropTable(name: "ATENDENTE");
            migrationBuilder.DropTable(name: "INTERESSE");
            migrationBuilder.DropTable(name: "ONG");
            migrationBuilder.DropTable(name: "STATUS");
            migrationBuilder.DropTable(name: "USUARIO");
            migrationBuilder.DropTable(name: "UF");
        }
    }
}
