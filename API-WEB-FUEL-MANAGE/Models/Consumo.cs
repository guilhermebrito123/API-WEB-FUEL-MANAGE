using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_WEB_FUEL_MANAGE.Models
{
    [Table("Consumos")]
    public class Consumo : LinkHATEOS
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Data  { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        [Required]
        public TipoCombustivel Tipo { get; set; }
        [Required]
        public int VeiculoId { get;}

        public Veiculo Veiculo { get; set; }

        //Eu não preciso de um campo Links que armazene os valores dos LinksDto do objeto, pois ao herdar da classe LinkHATEOS, a classe Consumo automaticamente já possui esse campo. Ela não será controlada no database
    }

    public enum TipoCombustivel
{
    Diesel,
    Etanol,
    Gasolina
}
    
}
