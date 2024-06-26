using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaMed.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Examinando_ExaminandoFK",
                table: "Processo");

            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Requisitante_RequisitanteFK",
                table: "Processo");

            migrationBuilder.DropIndex(
                name: "IX_Processo_ExaminandoFK",
                table: "Processo");

            migrationBuilder.DropIndex(
                name: "IX_Processo_RequisitanteFK",
                table: "Processo");

            migrationBuilder.DropColumn(
                name: "ExaminandoFK",
                table: "Processo");

            migrationBuilder.DropColumn(
                name: "RequisitanteFK",
                table: "Processo");

            migrationBuilder.AddColumn<int>(
                name: "ProcessoFK",
                table: "Requisitante",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExaminandoIdExa",
                table: "Processo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequisitanteIdReq",
                table: "Processo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessoFK",
                table: "Examinando",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requisitante_ProcessoFK",
                table: "Requisitante",
                column: "ProcessoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_ExaminandoIdExa",
                table: "Processo",
                column: "ExaminandoIdExa");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_RequisitanteIdReq",
                table: "Processo",
                column: "RequisitanteIdReq");

            migrationBuilder.CreateIndex(
                name: "IX_Examinando_ProcessoFK",
                table: "Examinando",
                column: "ProcessoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Examinando_Processo_ProcessoFK",
                table: "Examinando",
                column: "ProcessoFK",
                principalTable: "Processo",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Examinando_ExaminandoIdExa",
                table: "Processo",
                column: "ExaminandoIdExa",
                principalTable: "Examinando",
                principalColumn: "IdExa");

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Requisitante_RequisitanteIdReq",
                table: "Processo",
                column: "RequisitanteIdReq",
                principalTable: "Requisitante",
                principalColumn: "IdReq");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitante_Processo_ProcessoFK",
                table: "Requisitante",
                column: "ProcessoFK",
                principalTable: "Processo",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examinando_Processo_ProcessoFK",
                table: "Examinando");

            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Examinando_ExaminandoIdExa",
                table: "Processo");

            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Requisitante_RequisitanteIdReq",
                table: "Processo");

            migrationBuilder.DropForeignKey(
                name: "FK_Requisitante_Processo_ProcessoFK",
                table: "Requisitante");

            migrationBuilder.DropIndex(
                name: "IX_Requisitante_ProcessoFK",
                table: "Requisitante");

            migrationBuilder.DropIndex(
                name: "IX_Processo_ExaminandoIdExa",
                table: "Processo");

            migrationBuilder.DropIndex(
                name: "IX_Processo_RequisitanteIdReq",
                table: "Processo");

            migrationBuilder.DropIndex(
                name: "IX_Examinando_ProcessoFK",
                table: "Examinando");

            migrationBuilder.DropColumn(
                name: "ProcessoFK",
                table: "Requisitante");

            migrationBuilder.DropColumn(
                name: "ExaminandoIdExa",
                table: "Processo");

            migrationBuilder.DropColumn(
                name: "RequisitanteIdReq",
                table: "Processo");

            migrationBuilder.DropColumn(
                name: "ProcessoFK",
                table: "Examinando");

            migrationBuilder.AddColumn<int>(
                name: "ExaminandoFK",
                table: "Processo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequisitanteFK",
                table: "Processo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Processo_ExaminandoFK",
                table: "Processo",
                column: "ExaminandoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_RequisitanteFK",
                table: "Processo",
                column: "RequisitanteFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Examinando_ExaminandoFK",
                table: "Processo",
                column: "ExaminandoFK",
                principalTable: "Examinando",
                principalColumn: "IdExa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Requisitante_RequisitanteFK",
                table: "Processo",
                column: "RequisitanteFK",
                principalTable: "Requisitante",
                principalColumn: "IdReq",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
