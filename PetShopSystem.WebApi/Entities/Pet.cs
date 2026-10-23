using PetShopSystem.WebApi.Enums;

namespace PetShopSystem.WebApi.Entities
{
    public class Pet
    {
        public int Id { get; set; } // Chave primária automática
        public string Nome { get; set; } = string.Empty;
        public double Peso { get; set; }
        public RacaPet Raca { get; set; }
    }
}
