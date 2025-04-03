using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WEB_FUEL_MANAGE.Models
{
    [Table("Veículos")]
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Placa { get; set; }
        [Required]
        public int AnoFabricacao { get; set; }//Por mais que o campo esteja como required, por ser do tipo int, caso não insira nada, ele registra como 0, por isso devo colocar if atributo == 0, badRequest()
        [Required]
        public int AnoModelo { get; set; }//Por mais que o campo esteja como required, por ser do tipo int, caso não insira nada, ele registra como 0, por isso devo colocar if atributo == 0, badRequest()

        public ICollection<Consumo> Consumos { get; set; }

    }
}
