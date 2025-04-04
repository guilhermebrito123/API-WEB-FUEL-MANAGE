using System.ComponentModel.DataAnnotations.Schema;

namespace API_WEB_FUEL_MANAGE.Models
{
    [Table("VeiculoUsuarios")]
    public class VeiculoUsuarios
    {
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }
        public int UsuarioId { get; set;}
        public Usuario Usuario { get; set; }
    }
}
