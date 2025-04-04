using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_WEB_FUEL_MANAGE.Models
{
    [Table("Usuarios")]
    public class Usuario : LinkHATEOS
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [JsonIgnore]//Sempre que for retornar dados, ele vai ignorar esse campo, mas cria um conflito ao tentar criar um novo usuário, dizendo que o campo password é obrigatório, por isso, deve-se criar a classe UsuarioDto
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
