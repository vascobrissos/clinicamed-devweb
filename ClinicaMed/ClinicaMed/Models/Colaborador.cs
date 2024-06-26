using System.ComponentModel.DataAnnotations;

namespace ClinicaMed.Models
{
    public class Colaborador
    {
        public Colaborador()
        {
            ListaReceita = new HashSet<Receita>();
            ListaProceColab = new HashSet<ProcessoColaborador>();
        }

        /// <summary>
        /// Identificador único de um colaborador, chave primária, inteiro tamanho max 10
        /// </summary>
        [Key]
        public int IdCol { get; set; }

        /// <summary>
        /// Username do colaborador para login, varchar(255)
        /// </summary>
        [StringLength(255)]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Nome de apresentação do colaborador, varchar(255) 
        /// </summary>
        [StringLength(255)]
        [Required]
        public string NomeApresentacao { get; set; }

        /// <summary>
        /// Nome do colaborador, varchar(100)
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Apelido do colaborador, varchar(100)
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Apelido { get; set; }

        /// <summary>
        /// Telemovel do colaborador
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "o {0} introduzido é inválido")] // o {0} referencia o "Telemóvel"
        [Required]
        public string Telemovel { get; set; }

        /// <summary>
        /// Sexo/Genero do Colaborador, por analisar, indeciso se usamos int ou string
        /// </summary>
        [Required]
        public int Sexo { get; set; }

        /// <summary>
        /// Data de nascimento do colaborador, tipo DateOnly
        /// </summary>
        [Required]
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// Pais do Colaborador
        /// </summary>
        [StringLength(100)]
        public string Pais { get; set; }

        /// <summary>
        /// Morada do Colaborador
        /// </summary>
        [StringLength(100)]
        public string Morada { get; set; }

        /// <summary>
        /// Codigo postal do colaborador
        /// </summary>
        [StringLength(20)]
        public string CodPostal { get; set; }

        /// <summary>
        /// Localidade do colaborador
        /// </summary>
        [StringLength(100)]
        public string Localidade { get; set; }

        /// <summary>
        /// Nacionalidade do Colaborador
        /// </summary>
        [StringLength(100)]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Numero de Identificação Fiscal do colaborador, deve possuir 9 digitos de 0 a 9 
        /// </summary>
        [StringLength(9)]
        [RegularExpression("[0-9]{9}", ErrorMessage = "o {0} introduzido é inválido")]
        [Required]
        public string Nif { get; set; }

        /// <summary>
        /// Estado civil do colaborador, indeciso se deve ser int ou string
        /// </summary>
        [Required]
        public int EstadoCivil { get; set; }

        /// <summary>
        /// Identifica o numero de ordem do colaborador
        /// </summary>
        public int NumOrdem { get; set; }

        /// <summary>
        /// atributo para funcionar como FK
        /// no relacionamento entre a 
        /// base de dados do 'negócio' 
        /// e a base de dados da 'autenticação'
        /// </summary>
        [StringLength(40)]
        public string UserId { get; set; }


        //Coleção que referencia a relação de tipo 1-N com a class Receita
        public ICollection<Receita> ListaReceita { get; set; }

        // Lista referente à tabela do relacionamento N-M entre Processo de nome ProcessoColaborador
        public ICollection<ProcessoColaborador> ListaProceColab { get; set; }

        //Lista que referencia a relação do tipo 1-N com a class Consulta
        public ICollection<Consulta> ListaConsulta { get; set; }
    }
}
