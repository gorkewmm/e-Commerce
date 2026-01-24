using MultiShop.Discount.Dtos;
using MultiShop.Discount.Entities;

namespace MultiShop.Discount.Services
{
    public interface IDiscountService
    {
        Task<List<ResultDiscountCouponDto>> GetAllDiscountCouponAsync();
        Task UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateCouponDto);
        Task CreateDiscountCouponAsync(CreateDiscountCouponDto createCouponDto);

        Task<GetByIdDiscountCouponDto> GetByIdDiscountCouponAsync(int id);

        Task DeleteDiscountCouponAsync(int id);
    }
}
