using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaMed.Migrations
{
    /// <inheritdoc />
    public partial class RelationReqProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitante_Processo_ProcessoFK",
                table: "Requisitante");

            migrationBuilder.DropIndex(
                name: "IX_Requisitante_ProcessoFK",
                table: "Requisitante");

            migrationBuilder.DropColumn(
                name: "ProcessoFK",
                table: "Requisitante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessoFK",
                table: "Requisitante",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requisitante_ProcessoFK",
                table: "Requisitante",
                column: "ProcessoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitante_Processo_ProcessoFK",
                table: "Requisitante",
                column: "ProcessoFK",
                principalTable: "Processo",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
