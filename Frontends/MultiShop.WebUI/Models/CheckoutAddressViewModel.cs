using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Models
{
    public class CheckoutAddressViewModel
    {
        public CreateOrderAddressDto OrderAddress { get; set; } = new CreateOrderAddressDto();

        /// <summary>Mevcut kargo müşteri kaydı varsa güncellenir (Cargo mikroservisi).</summary>
        public int? ExistingCargoCustomerId { get; set; }
    }
}
