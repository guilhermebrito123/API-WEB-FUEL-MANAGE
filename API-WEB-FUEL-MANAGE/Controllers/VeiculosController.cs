﻿using API_WEB_FUEL_MANAGE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WEB_FUEL_MANAGE.Controllers
{
    [Authorize]//Agora todas as minhas roas do meu controlador só são acessíveis por autenticação
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos.ToListAsync();

            return Ok(model);//Esse método retorna o status 200
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
            if(model.AnoFabricacao <= 0 || model.AnoModelo <= 0)
            {
                return BadRequest(new {message = "Ano de fabricação e ano de modelo são obrigatórios devem ser maiores que 0"});
            }

            _context.Veiculos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new {id = model.Id}, model);//O método createdataction serve para retornar o status 201 "Created". no cabeçalho da resposta, aparece o caminho para acessar as informações do veiculo cadastrado
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Veiculos.Include(t => t.Consumos)//Para recuperar o array Consumos relacionado ao veículo específico, aso contrário retorna como null
                .Include(t => t.Usuarios).ThenInclude(t => t.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(model == null) return NotFound();

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Veiculo model)
        {
            if(id != model.Id) return BadRequest();

            var modelDb = await _context.Veiculos.AsNoTracking().//Utilizamos o AsNoTracking para desvincular o monitoramento de dados feitos no modelDb,
                FirstOrDefaultAsync(c => c.Id == id);//fazendo apenas o processo de leitura, como apenas quero verificar se existe dados do id = n, só preciso da função de leitura, não preciso monitorá-los
                                                     //(é mais ou menos isso)


            if (modelDb == null) return NotFound();

            _context.Veiculos.Update(model);
            await _context.SaveChangesAsync();//É aqui onde eu atualizo os dados
            return NoContent();//Não quero retornar nada.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Veiculos.FindAsync(id);

            if(model == null) return NotFound();

            _context.Veiculos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void GerarLinks(Veiculo model)
        {
            //Não há porque colocar o método POST porque são informações de manipulação de dados de um objeto, eu posso recuperar, deletar ou atualizar o objeto específico. Não posso "postar" o objeto específico já cadastrado.
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "Delete"));
        }

        //Funcionalidade de adicionar um usuário a um veículo em questão:

        [HttpPost("{id}/usuarios")]//Posso fazer assim para quando for associar um recurso a outro. Esse id é do veículo
        public async Task<ActionResult> AddUsuario(int id, VeiculoUsuarios model)//Para esse id passado, estou associando a um usuário utilizando o VeiculoUsuarios
        {
            if(id != model.VeiculoId) return BadRequest();

            _context.VeiculosUsuarios.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = model.VeiculoId }, model);
        }//No parâmetro "GetById" passado para a função CreatedAtAction, eu quero visualizar os dados dos usuários que estão associados a esse veículo
         //Então adiciono .Include(t => t.Usuarios).ThenInclude(t => t.Usuario) no método GetById, onde Usuarios é uma tabela do tipo
         //VeiculoUsuarios, onde eu só tenho a informação do Id do usuário associado ao veículo. Caso queira adicionar outras informações, faço o ThenInclude(t => t.usuario)

        //Remover associação de usuário ao veículo:

        [HttpDelete("{id}/usuarios/{usuarioId}")]
        public async Task<ActionResult> DeleteUsuario(int id, int usuarioId)
        {
            var model = await _context.VeiculosUsuarios.Where(c => c.VeiculoId == id && c.UsuarioId == usuarioId)
            .FirstOrDefaultAsync();

            if(model == null) return NotFound();

            _context.VeiculosUsuarios.Remove(model);    
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
