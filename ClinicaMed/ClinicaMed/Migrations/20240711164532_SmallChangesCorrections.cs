using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaMed.Migrations
{
    /// <inheritdoc />
    public partial class SmallChangesCorrections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita");

            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Processo_ProcessoFK",
                table: "Receita");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessoFK",
                table: "Receita",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ColaboradorFK",
                table: "Receita",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita",
                column: "ColaboradorFK",
                principalTable: "Colaborador",
                principalColumn: "IdCol");

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Processo_ProcessoFK",
                table: "Receita",
                column: "ProcessoFK",
                principalTable: "Processo",
                principalColumn: "IdPro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita");

            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Processo_ProcessoFK",
                table: "Receita");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessoFK",
                table: "Receita",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ColaboradorFK",
                table: "Receita",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita",
                column: "ColaboradorFK",
                principalTable: "Colaborador",
                principalColumn: "IdCol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Processo_ProcessoFK",
                table: "Receita",
                column: "ProcessoFK",
                principalTable: "Processo",
                principalColumn: "IdPro",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
