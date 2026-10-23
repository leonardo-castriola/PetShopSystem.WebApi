using Microsoft.EntityFrameworkCore;
using PetShopSystem.WebApi.Data;
using PetShopSystem.WebApi.Enums;
using System;
using System.Threading.Tasks;

namespace PetShopSystem.WebApi.Services
{
    public class BanhoService : IBanhoService
    {
        private readonly AppDbContext _context;

        // O Entity Framework é injetado automaticamente aqui pelo ASP.NET Core
        public BanhoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalcularValorBanhoPorIdAsync(int petId)
        {
            // 1. Busca o pet no banco de dados SQLite de forma assíncrona
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);

            // 2. Se o pet não for encontrado, lança uma exceção tratável
            if (pet == null)
            {
                throw new KeyNotFoundException($"Pet com o ID {petId} não foi localizado no sistema.");
            }

            decimal valorBase;

            // 3. Aplica a regra por faixa de peso que você desenvolveu
            if (pet.Peso <= 10.0)
            {
                valorBase = 40.00m;
            }
            else if (pet.Peso <= 25.0)
            {
                valorBase = 60.00m;
            }
            else
            {
                valorBase = 80.00m;
            }

            // 4. Aplica o bônus de raça
            if (pet.Raca == RacaPet.Poodle)
            {
                valorBase += 10.00m;
            }

            return valorBase;
        }
    }
}
