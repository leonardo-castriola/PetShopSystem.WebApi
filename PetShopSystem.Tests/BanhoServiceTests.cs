using Microsoft.EntityFrameworkCore;
using PetShopSystem.WebApi.Data;
using PetShopSystem.WebApi.Entities;
using PetShopSystem.WebApi.Enums;
using PetShopSystem.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PetShopSystem.Tests
{
    public class BanhoServiceTests
    {
        // Método auxiliar para criar um banco de dados temporário isolado para cada teste
        private AppDbContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nome aleatório garante isolamento
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CalcularBanho_DeveRetornar40_QuandoPetForPequenoENaoPoodle()
        {
            // Arrange (Preparação dos dados)
            var context = CriarContextoEmMemoria();
            var pet = new Pet { Id = 1, Nome = "Mel", Peso = 8.5, Raca = RacaPet.ViraLata };
            context.Pets.Add(pet);
            await context.SaveChangesAsync();

            var service = new BanhoService(context);

            // Act (Execução da regra de negócio)
            decimal valorFinal = await service.CalcularValorBanhoPorIdAsync(1);

            // Assert (Verificação do resultado esperado)
            Assert.Equal(40.00m, valorFinal);
        }

        [Fact]
        public async Task CalcularBanho_DeveAdicionarDezReais_QuandoPetForPoodle()
        {
            // Arrange
            var context = CriarContextoEmMemoria();
            var pet = new Pet { Id = 2, Nome = "Fred", Peso = 12.0, Raca = RacaPet.Poodle }; // Poodle médio (Base 60 + 10 bônus)
            context.Pets.Add(pet);
            await context.SaveChangesAsync();

            var service = new BanhoService(context);

            // Act
            decimal valorFinal = await service.CalcularValorBanhoPorIdAsync(2);

            // Assert
            Assert.Equal(70.00m, valorFinal);
        }

        [Fact]
        public async Task CalcularBanho_DeveLancarExcecao_QuandoIdNaoExistirNoBanco()
        {
            // Arrange
            var context = CriarContextoEmMemoria(); // Banco vazio, sem nenhum pet cadastrado
            var service = new BanhoService(context);

            // Act & Assert
            // Verificamos se o sistema joga a exceção correta de "Id Não Encontrado"
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.CalcularValorBanhoPorIdAsync(999);
            });
        }
    }
}
