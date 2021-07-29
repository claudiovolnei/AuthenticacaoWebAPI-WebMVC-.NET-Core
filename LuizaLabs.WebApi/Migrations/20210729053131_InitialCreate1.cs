using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuizaLabs.WebApi.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecuperacaoSenhaUsuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenhaAnterior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenhaNova = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmacaoSenhaNova = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    HorarioSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecuperacaoSenhaUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecuperacaoSenhaUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecuperacaoSenhaUsuario_UsuarioId",
                table: "RecuperacaoSenhaUsuario",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecuperacaoSenhaUsuario");
        }
    }
}
