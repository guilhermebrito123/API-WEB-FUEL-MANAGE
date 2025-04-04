using API_WEB_FUEL_MANAGE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WEB_FUEL_MANAGE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsumosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Consumos.ToListAsync();

            return Ok(model);//Esse método retorna o status 200
        }

        [HttpPost]
        public async Task<ActionResult> Create(Consumo model)
        {
            _context.Consumos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = model.Id }, model);//O método createdataction serve para retornar o status 201 "Created". no cabeçalho da resposta, aparece o caminho para acessar as informações do veiculo cadastrado
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Consumos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound();

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Consumo model)
        {
            if (id != model.Id) return BadRequest();

            var modelDb = await _context.Consumos.AsNoTracking().//Utilizamos o AsNoTracking para desvincular o monitoramento de dados feitos no modelDb,
                FirstOrDefaultAsync(c => c.Id == id);//fazendo apenas o processo de leitura, como apenas quero verificar se existe dados do id = n, só preciso da função de leitura, não preciso monitorá-los
                                                     //(é mais ou menos isso)


            if (modelDb == null) return NotFound();

            _context.Consumos.Update(model);
            await _context.SaveChangesAsync();//É aqui onde eu atualizo os dados
            return NoContent();//Não quero retornar nada.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Consumos.FindAsync(id);

            if (model == null) return NotFound();

            _context.Consumos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private void GerarLinks(Consumo model)
        {
            //Não há porque colocar o método POST porque são informações de manipulação de dados de um objeto, eu posso recuperar, deletar ou atualizar o objeto específico. Não posso "postar" o objeto específico já cadastrado.
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "Delete"));
        }
    }
}
