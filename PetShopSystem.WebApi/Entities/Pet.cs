using System.ComponentModel.DataAnnotations;
using PetShopSystem.WebApi.Enums;

namespace PetShopSystem.WebApi.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O nome do pet é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do pet deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // AJUSTE ESTA LINHA EXATAMENTE ASSIM:
        [Range(0.01, 500.0, ErrorMessage = "O peso deve ser maior que zero e no máximo 500 kg.")]
        public double Peso { get; set; }

        [EnumDataType(typeof(RacaPet), ErrorMessage = "A raça informada é inválida.")]
        public RacaPet Raca { get; set; }
    }
}
