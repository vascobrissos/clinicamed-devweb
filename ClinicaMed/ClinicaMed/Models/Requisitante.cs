using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaMed.Models
{
    public class Requisitante
    {
        public Requisitante()
        {
        }

        /// <summary>
        /// Identificador do requisitante
        /// </summary>
        [Key]
        public int IdReq { get; set; }

        /// <summary>
        /// Nome do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Apelido do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Apelido { get; set; }

        /// <summary>
        /// Telemovel do requisitante
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "o {0} introduzido é inválido")] // o {0} referencia o "Telemóvel"
        [Required]
        public string Telemovel { get; set; }

        /// <summary>
        /// Email do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Sexo do requisitante
        /// </summary>
        public int Sexo { get; set; }

        /// <summary>
        /// Pais do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Pais { get; set; }

        /// <summary>
        /// Morada do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Morada { get; set; }

        /// <summary>
        /// Código Postal do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string CodigoPostal { get; set; }

        /// <summary>
        /// Localidade do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Localidade { get; set; }

        /// <summary>
        /// Nacionalidade do requisitante
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Numero de Identificação Fiscal do requisitante, deve possuir 9 digitos de 0 a 9 
        /// </summary>
        [StringLength(9)]
        [RegularExpression("[0-9]{9}", ErrorMessage = "o {0} introduzido é inválido")]
        [Required]
        public string Nif { get; set; }

        public int ProcessoId { get; set; }
        public Processo? Processo { get; set; }
    }
}