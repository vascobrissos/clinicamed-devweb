using System.ComponentModel.DataAnnotations;

namespace ClinicaMed.Models
{
    public class Examinando
    {
        public Examinando()
        {
            ListaProcesso = new HashSet<Processo>();
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
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Apelido do examinando
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Apelido { get; set; }

        /// <summary>
        /// Telemovel do examinando
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "o {0} introduzido é inválido")] // o {0} referencia o "Telemóvel"
        public string Telemovel { get; set; }

        /// <summary>
        /// Email do examinando
        /// </summary>
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Sexo do examinando
        /// </summary>
        public int Sexo { get; set; }

        /// <summary>
        /// Notas sobre os antecedentes do examinando
        /// </summary>
        [StringLength(10000)]
        public string Antecedentes { get; set; }

        /// <summary>
        /// Data de nascimento do examinando
        /// </summary>
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// Profissão do examinando
        /// </summary>
        [StringLength(100)]
        public string Profissao { get; set; }

        /// <summary>
        /// Pais do examinando
        /// </summary>
        [StringLength(100)]
        public string Pais { get; set; }

        /// <summary>
        /// Morada do examinando
        /// </summary>
        [StringLength(100)]
        public string Morada { get; set; }

        /// <summary>
        /// Código Postal do examinando
        /// </summary>
        [StringLength(10)]
        public string CodigoPostal { get; set; }

        /// <summary>
        /// Localidade do examinando
        /// </summary>
        [StringLength(100)]
        public string Localidade { get; set; }

        /// <summary>
        /// Nacionalidade do examinando
        /// </summary>
        [StringLength(100)]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Numero de utente do examinando
        /// </summary>
        public int NumUtente { get; set; }

        /// <summary>
        /// Cartão de Cidadão do examinando
        /// </summary>
        [Display(Name = "Cartão de Cidadão")]
        [StringLength(8)]
        [RegularExpression("[0-9]{8}", ErrorMessage = "o {0} introduzido é inválido")] // o {0} referencia o "Cartão de Cidadão"
        public string CartaoCidadao { get; set; }

        /// <summary>
        /// Data de validade do cartão de cidadão
        /// </summary>
        public DateOnly ValidadeCC { get; set; }

        /// <summary>
        /// Numero de Identificação Fiscal do examinando, deve possuir 9 digitos de 0 a 9 
        /// </summary>
        [StringLength(9)]
        [RegularExpression("[0-9]{9}", ErrorMessage = "o {0} introduzido é inválido")]
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
        public string NumeroSeguro { get; set; }

        /// <summary>
        /// Filiacao da mae do examinando
        /// </summary>
        [StringLength(255)]
        public string FiliacaoMae { get; set; }

        /// <summary>
        /// Filiacao do pai do examinando
        /// </summary>
        [StringLength(255)]
        public string FiliacaoPai { get; set; }

        // Lista referente à relação 1-N entre a tabela Examinando com a tabela Processos 
        public ICollection<Processo> ListaProcesso { get; set; }
    }
}