using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaMed.Migrations
{
    /// <inheritdoc />
    public partial class Teste2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita");

            migrationBuilder.AddColumn<int>(
                name: "ColaboradorIdCol",
                table: "Receita",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receita_ColaboradorIdCol",
                table: "Receita",
                column: "ColaboradorIdCol");

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita",
                column: "ColaboradorFK",
                principalTable: "Colaborador",
                principalColumn: "IdCol",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorIdCol",
                table: "Receita",
                column: "ColaboradorIdCol",
                principalTable: "Colaborador",
                principalColumn: "IdCol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita");

            migrationBuilder.DropForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorIdCol",
                table: "Receita");

            migrationBuilder.DropIndex(
                name: "IX_Receita_ColaboradorIdCol",
                table: "Receita");

            migrationBuilder.DropColumn(
                name: "ColaboradorIdCol",
                table: "Receita");

            migrationBuilder.AddForeignKey(
                name: "FK_Receita_Colaborador_ColaboradorFK",
                table: "Receita",
                column: "ColaboradorFK",
                principalTable: "Colaborador",
                principalColumn: "IdCol");
        }
    }
}
