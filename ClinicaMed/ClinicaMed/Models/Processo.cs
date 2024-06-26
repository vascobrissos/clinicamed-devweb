﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaMed.Models
{
    public class Processo
    {
        public Processo()
        {
            ListaProceColab = new HashSet<ProcessoColaborador>();
            ListaReceita = new HashSet<Receita>();
        }

        /// <summary>
        /// Identificador único do processo
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador interno do processo
        /// </summary>
        [StringLength(100)]
        public string IdInterno { get; set; }

        /// <summary>
        /// Data da criação do processo
        /// </summary>
        public  DateTime DataCriacao { get; set; }

        /// <summary>
        /// Data do inicio do processo
        /// </summary>
        public DateOnly DataInicio { get; set; }

        /// <summary>
        /// Data do termino do processo
        /// </summary>
        public DateOnly DataTermino { get; set; }

        /// <summary>
        /// Estado do processo
        /// </summary>
        public int Estado { get; set; }

        // Lista referente á tabela do relacionamento N-M entre Colaborador de nome ProcessoColaborador
        public ICollection<ProcessoColaborador> ListaProceColab {  get; set; } 

        //Lista que referencia a relação do tipo 1-N com a class Receita
        public ICollection<Receita> ListaReceita { get; set; }

        //Lista que referencia a relação do tipo 1-N com a class Consulta
        public ICollection<Consulta> ListaConsulta { get; set; }

        //**************** Chave estrangeira com a tabela Examinando ***************//

        [ForeignKey(nameof(Examinando))]
        public int ExaminandoFK { get; set; }
        public Examinando Examinando {  get; set; }

        //**************** Chave estrangeira com a tabela Requisitante ***************//

        [ForeignKey(nameof(Requisitante))]
        public int RequisitanteFK { get; set; }
        public Requisitante Requisitante { get; set; }
    }
}
