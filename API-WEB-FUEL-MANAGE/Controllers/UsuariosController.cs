using API_WEB_FUEL_MANAGE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_WEB_FUEL_MANAGE.Controllers
{
    [Authorize(Roles = "Administrador")]//Agora todas as minhas roas do meu controlador só são acessíveis por autenticação
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Usuarios.ToListAsync();

            return Ok(model);//Esse método retorna o status 200
        }

        [HttpPost]
        public async Task<ActionResult> Create(UsuarioDto model)
        {
            Usuario novo = new Usuario()
            {
                Nome = model.Nome,
                Password = model.Password,
                Perfil = model.Perfil
            };


            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _context.Usuarios.Add(novo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = novo.Id }, novo);//O método createdataction serve para retornar o status 201 "Created". no cabeçalho da resposta, aparece o caminho para acessar as informações do veiculo cadastrado
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Usuarios
                .FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound();

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UsuarioDto model)
        {
            if (id != model.Id) return BadRequest();

            var modelDb = await _context.Usuarios.AsNoTracking().//Utilizamos o AsNoTracking para desvincular o monitoramento de dados feitos no modelDb,
                FirstOrDefaultAsync(c => c.Id == id);//fazendo apenas o processo de leitura, como apenas quero verificar se existe dados do id = n, só preciso da função de leitura, não preciso monitorá-los
                                                     //(é mais ou menos isso)


            if (modelDb == null) return NotFound();

            modelDb.Nome = model.Nome;
            modelDb.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            modelDb.Perfil = modelDb.Perfil;

            _context.Usuarios.Update(modelDb);
            await _context.SaveChangesAsync();//É aqui onde eu atualizo os dados
            return NoContent();//Não quero retornar nada.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Usuarios.FindAsync(id);

            if (model == null) return NotFound();

            _context.Usuarios.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Função para gerar o token de autenticação. Com ela, posso acessar as informações geradas pelos métodos.
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<ActionResult> Authenticate(AuthenticateDto model)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(model.Id);

            if(usuarioDb == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuarioDb.Password))
                return Unauthorized();

            var jwt = GenerateJwtToken(usuarioDb);//Preciso gerar esse jwt, para isso faço a configuração na classe logo abaixo

            return Ok(new {jwtToken = jwt});
        }

        //Função de autenticação:
        private string GenerateJwtToken(Usuario model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Ry74cBQva5dThwbwchR9jhbtRFnJxWSZ");
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.Role, model.Perfil.ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void GerarLinks(Usuario model)
        {
            //Não há porque colocar o método POST porque são informações de manipulação de dados de um objeto, eu posso recuperar, deletar ou atualizar o objeto específico. Não posso "postar" o objeto específico já cadastrado.
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "Delete"));
        }
    }
}
