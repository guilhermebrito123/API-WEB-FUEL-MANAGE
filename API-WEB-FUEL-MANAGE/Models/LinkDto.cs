using System.Collections.Specialized;

namespace API_WEB_FUEL_MANAGE.Models
{
    public class LinkDto //(Data transfer Objects) -> Não é um objeto de banco de dados, não tem impacto no bd, não é associado ao modelo de dados, são só informações. É um objeto para transferir dados
    {
        public int Id { get; set; }
        public string Href { get; set; }//Propriedade que armazena os links que serão utilizados para navegar
        public string Rel { get; set; }//Relacionamento, qual é a ação que está sendo utilizada no seu objeto
        public string Metodo { get; set; }//Método http que está sendo usado

        public LinkDto(int id, string href, string rel, string metodo)
        {
            Id = id;
            Href = href;
            Rel = rel;
            Metodo = metodo;
        }

    }

    public class LinkHATEOS
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
