using System.ComponentModel.DataAnnotations;

namespace API_WEB_FUEL_MANAGE.Models
{
    public class UsuarioDto //Essa classe é usada como parãmetro de entrada de dados para as ações do controlador de usuarios, mas os dados incluídos nela serão armazenados na classe Uusarios, só para assim burlar o comflito de criação de Usuario
    {
        public int? Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Perfil Perfil { get; set; }
    }
}
