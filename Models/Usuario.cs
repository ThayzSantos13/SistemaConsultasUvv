using System.ComponentModel.DataAnnotations;

namespace SistemaConsultasUVV.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ICollection<Consulta>? Consultas { get; set; }
    }
}