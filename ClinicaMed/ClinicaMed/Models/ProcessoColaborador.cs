using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaMed.Models
{
    [PrimaryKey(nameof(Processo), nameof(Colaborador))]
    public class ProcessoColaborador
    {
        /// <summary>
        /// Identificador ùnico da associação entre Processo e Colaborador
        /// </summary>
        [Key]
        public int IdProCol { get; set; }

        /// <summary>
        /// Data da atribuicao do processo pelo colaborador
        /// </summary>
        [Required]
        public DateTime DataAtribuicao { get; set; }

        /// <summary>
        /// Data da remoção do processo pelo colaborador
        /// </summary>
        [Required]
        public DateTime DataRemocao { get; set; }

        //******************************************************//
        // Chaves forasteiras das tabelas Colaborador e Processo//
        //******************************************************//

        [ForeignKey(nameof(Processo))]
        public int ProcessoFK { get; set; }
        public Processo Processo { get; set; }

        [ForeignKey(nameof(Colaborador))]
        public int ColaboradorFK { get; set; }
        public Colaborador Colaborador { get; set; }
    }
}
