using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaMed.Models
{
    public class Receita
    {
        /// <summary>
        /// Identificador único da receita, chave primária
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Numero da receita?
        /// </summary>
        [StringLength(150)]
        public string NumReceita { get; set; }

        /// <summary>
        /// Notas/Observações sobre a receita
        /// </summary>
        [StringLength(1000)]
        public string Notas { get; set; }

        /// <summary>
        /// Data e hora da receita
        /// </summary>
        public DateTime DataReceita { get; set; }

        /// <summary>
        /// Identificador do Processo referente à receita
        /// </summary>
        [ForeignKey(nameof(Processo))]
        public int ProcessoFK { get; set; }
        public Processo Processo { get; set; }

        /// <summary>
        /// Identificador do Colaborador referente à receita
        /// </summary>
        // Deve ser uma chave estrangeira com a class Colaborador
        [ForeignKey(nameof(Colaborador))]
        public int ColaboradorFK { get; set; }
        public Colaborador Colaborador { get; set; }

        /// <summary>
        /// Identifica o estado da receita. REVER CONCEITO
        /// </summary>
        public int Estado { get; set; }
    }
}
