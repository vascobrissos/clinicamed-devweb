using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaMed.Models
{
    public class Examinando
    {
        public Examinando()
        {
        }

        /// <summary>
        /// Identificador ùnico do examinando na clínica
        /// </summary>
        [Key]
        public int IdExa { get; set; }

        /// <summary>
        /// Nome do examinando
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Apelido do examinando
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        [Display(Name = "Apelido")]
        public string Apelido { get; set; }

        /// <summary>
        /// Telemovel do examinando
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "o {0} introduzido é inválido")] // o {0} referencia o "Telemóvel"
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        public string Telemovel { get; set; }

        /// <summary>
        /// Email do examinando
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Sexo do examinando
        /// </summary>
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        [Display(Name = "Sexo")]
        public int Sexo { get; set; }

        /// <summary>
        /// Notas sobre os antecedentes do examinando
        /// </summary>
        [StringLength(10000)]
        [Display(Name = "Antecedentes")]
        public string Antecedentes { get; set; }

        /// <summary>
        /// Data de nascimento do examinando
        /// </summary>
        [Required(ErrorMessage = "A {0} é obrigatória!")]
        [Display(Name = "Data de Nascimento")]
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// Profissão do examinando
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Profissão")]
        [Required(ErrorMessage = "A {0} é obrigatória!")]
        public string Profissao { get; set; }

        /// <summary>
        /// Pais do examinando
        /// </summary>
        [StringLength(100)]
        [Display(Name = "País")]
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        public string Pais { get; set; }

        /// <summary>
        /// Morada do examinando
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "A {0} é obrigatória!")]
        public string Morada { get; set; }

        /// <summary>
        /// Código Postal do examinando
        /// </summary>
        [StringLength(10)]
        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        public string CodigoPostal { get; set; }

        /// <summary>
        /// Localidade do examinando
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "A {0} é obrigatória!")]
        public string Localidade { get; set; }

        /// <summary>
        /// Nacionalidade do examinando
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "A {0} é obrigatória!")]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Numero de utente do examinando
        /// </summary>
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        [Display(Name = "Número de Utente")]
        public int NumUtente { get; set; }

        /// <summary>
        /// Cartão de Cidadão do examinando
        /// </summary>
        [Display(Name = "Cartão de Cidadão")]
        [StringLength(8)]
        [RegularExpression("[0-9]{8}", ErrorMessage = "o {0} introduzido é inválido")] // o {0} referencia o "Cartão de Cidadão"
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        public string CartaoCidadao { get; set; }

        /// <summary>
        /// Data de validade do cartão de cidadão
        /// </summary>
        [Required(ErrorMessage = "A {0} é obrigatória!")]
        [Display(Name = "Validade do CC")]
        public DateOnly ValidadeCC { get; set; }

        /// <summary>
        /// Numero de Identificação Fiscal do examinando, deve possuir 9 digitos de 0 a 9 
        /// </summary>
        [StringLength(9)]
        [RegularExpression("[0-9]{9}", ErrorMessage = "o {0} introduzido é inválido")]
        [Required(ErrorMessage = "O {0} é obrigatório!")]
        public string Nif { get; set; }

        /// <summary>
        /// Seguradora do examinando
        /// </summary>
        [StringLength(100)]
        public string Seguradora { get; set; }

        /// <summary>
        /// Numero de seguro do examinando
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Número de Seguro")]
        public string NumeroSeguro { get; set; }

        /// <summary>
        /// Filiacao da mae do examinando
        /// </summary>
        [StringLength(255)]
        [Display(Name = "Filiação Mãe")]
        public string FiliacaoMae { get; set; }

        /// <summary>
        /// Filiacao do pai do examinando
        /// </summary>
        [StringLength(255)]
        [Display(Name = "Filiação Pai")]
        public string FiliacaoPai { get; set; }

        public int ProcessoId { get; set; }
        public Processo? Processo { get; set; }
    }
}