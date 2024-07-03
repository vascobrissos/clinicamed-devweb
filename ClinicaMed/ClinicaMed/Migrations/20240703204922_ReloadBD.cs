using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaMed.Migrations
{
    /// <inheritdoc />
    public partial class teste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Examinando_ExaminandoIdExa",
                table: "Processo");

            migrationBuilder.AlterColumn<int>(
                name: "ExaminandoIdExa",
                table: "Processo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Examinando_ExaminandoIdExa",
                table: "Processo",
                column: "ExaminandoIdExa",
                principalTable: "Examinando",
                principalColumn: "IdExa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Examinando_ExaminandoIdExa",
                table: "Processo");

            migrationBuilder.AlterColumn<int>(
                name: "ExaminandoIdExa",
                table: "Processo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Examinando_ExaminandoIdExa",
                table: "Processo",
                column: "ExaminandoIdExa",
                principalTable: "Examinando",
                principalColumn: "IdExa",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
