using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaMed.Models
{
    public class Consulta
    {
        /// <summary>
        /// Identificado unico da consulta
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data da consulta
        /// </summary>
        public DateTime DataConsulta { get; set; }

        /// <summary>
        /// Observacoes uteis pro parte do colaborador sobre a consulta
        /// </summary>
        [StringLength(100)]
        public string Observacoes { get; set; }

        /// <summary>
        /// Estado da consulta
        /// </summary>
        public int Estado { get; set; }

        //******************************************************//
        // Chaves forasteiras das tabelas Colaborador e Processo//
        //******************************************************//

        [ForeignKey(nameof(Colaborador))]
        public int ColaboradorFK { get; set; }
        public Colaborador Colaborador { get; set; }

        [ForeignKey(nameof(Processo))]
        public int ProcessoFK { get; set; }
        public Colaborador Processo { get; set; }
    }
}
