using System.Threading.Tasks;

namespace PetShopSystem.WebApi.Services
{
    public interface IBanhoService
    {
        // Usamos Task e async para operações assíncronas de banco de dados, padrão de mercado
        Task<decimal> CalcularValorBanhoPorIdAsync(int petId);
    }
}
