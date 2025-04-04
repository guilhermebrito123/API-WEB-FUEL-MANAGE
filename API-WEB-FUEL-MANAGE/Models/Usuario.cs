using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WEB_FUEL_MANAGE.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Perfil Perfil { get; set; }

        public ICollection<VeiculoUsuarios> Veiculos { get; set; }

        //Como um usuario pode ter um ou n veículos e um veículo pode estar associado a um oou n usuários, preciso criar uma classe model que corresponda a esse relacionamento n para n e por fim devo configurar no appDbContext no construtor

    }

    public enum Perfil
    {
        [Display(Name = "Administrador")]
        Administrador,
        [Display(Name = "Usuario")]
        Usuario
    }
}
