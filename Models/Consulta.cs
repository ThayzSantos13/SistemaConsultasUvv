using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaConsultasUVV.Models
{
    public class Consulta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required]
        public DateTime DataHora { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}
