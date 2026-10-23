using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShopSystem.WebApi.Data;
using PetShopSystem.WebApi.Entities;
using PetShopSystem.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetShopSystem.WebApi.Controllers
{
    public record CalculoBanhoResponse(int PetId, decimal ValorBanho);

    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBanhoService _banhoService;
        private readonly ILogger<PetController> _logger;

        // Injetando o banco de dados e o serviço de negócio no construtor
        public PetController(AppDbContext context, IBanhoService banhoService, ILogger<PetController> logger)
        {
            _context = context;
            _banhoService = banhoService;
            _logger = logger;
        }

        // 1. POST: api/pet (Cadastrar um novo Pet)
        [HttpPost("Cadastrar um novo Pet")]
        public async Task<ActionResult<Pet>> CriarPet([FromBody] Pet novoPet)
        {
            novoPet.Id = 0;
            novoPet.Nome = novoPet.Nome.Trim();

            _context.Pets.Add(novoPet);
            await _context.SaveChangesAsync();

            // Retorna o status 201 Created com a rota para buscar o pet criado
            return CreatedAtAction(nameof(ObterPorId), new { id = novoPet.Id }, novoPet);
        }

        // 2. GET: api/pet (Listar todos os Pets)
        [HttpGet("Listar todos os Pets")]
        public async Task<ActionResult<IEnumerable<Pet>>> ObterTodos()
        {
            var pets = await _context.Pets.AsNoTracking().ToListAsync();
            return Ok(pets);
        }

        // 3. GET: api/pet/{id} (Buscar um Pet específico por ID)
        [HttpGet("{id:int}/Buscar um Pet específico por ID")]
        public async Task<ActionResult<Pet>> ObterPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensagem = "O ID do pet deve ser maior que zero." });
            }

            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                return NotFound(new { mensagem = $"Pet com o ID {id} não foi encontrado." });
            }

            return Ok(pet);
        }
       
        // 4. GET: api/pet/{id}/calcular-banho (Calcular o banho do Pet)
        [HttpGet("{id:int}/Calcular o banho do Pet")]
        public async Task<ActionResult<CalculoBanhoResponse>> CalcularBanho(int id)
        {
            try
            {
                decimal valorTotal = await _banhoService.CalcularValorBanhoPorIdAsync(id);
                return Ok(new CalculoBanhoResponse(id, valorTotal));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao calcular o banho do pet {PetId}.", id);
                return Problem(statusCode: 500, title: "Não foi possível calcular o valor do banho.");
            }
        }

        // 5. PUT: api/pet/{id} (Atualizar os dados de um Pet existente)
        [HttpPut("{id:int}/Atualizar um Pet existente")]
        public async Task<ActionResult<Pet>> AtualizarPet(int id, [FromBody] Pet petAtualizado)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensagem = "O ID do pet deve ser maior que zero." });
            }

            if (petAtualizado.Id != 0 && petAtualizado.Id != id)
            {
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID enviado no corpo da requisição." });
            }

            var petExistente = await _context.Pets.FindAsync(id);

            if (petExistente == null)
            {
                return NotFound(new { mensagem = $"Pet com o ID {id} não foi encontrado." });
            }

            petExistente.Nome = petAtualizado.Nome.Trim();
            petExistente.Peso = petAtualizado.Peso;
            petExistente.Raca = petAtualizado.Raca;

            await _context.SaveChangesAsync();

            return Ok(petExistente);
        }

        // 6. DELETE: api/pet/{id} (Remover um Pet pelo ID)
        [HttpDelete("{id:int}/Remover um Pet pelo ID")]
        public async Task<IActionResult> DeletarPet(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensagem = "O ID do pet deve ser maior que zero." });
            }

            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                return NotFound(new { mensagem = $"Pet com o ID {id} não foi encontrado." });
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }        
    }
}
