using MultiShop.Basket.Dtos;
using MultiShop.Basket.Settings;
using System.Collections.Generic;
using System.Text.Json;

namespace MultiShop.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task DeleteBasket(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);
        }

        public async Task<BasketTotalDto> GetBasket(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new BasketTotalDto { UserId = userId, BasketItems = new List<BasketItemDto>() };
            }

            var existBasket = await _redisService.GetDb().StringGetAsync(userId);
            if (!existBasket.HasValue)
            {
                return new BasketTotalDto { UserId = userId, BasketItems = new List<BasketItemDto>() };
            }

            try
            {
                var deserialized = JsonSerializer.Deserialize<BasketTotalDto>(existBasket);
                if (deserialized == null)
                {
                    return new BasketTotalDto { UserId = userId, BasketItems = new List<BasketItemDto>() };
                }

                deserialized.BasketItems ??= new List<BasketItemDto>();
                return deserialized;
            }
            catch
            {
                return new BasketTotalDto { UserId = userId, BasketItems = new List<BasketItemDto>() };
            }
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            await _redisService.GetDb().StringSetAsync(basketTotalDto.UserId, JsonSerializer.Serialize(basketTotalDto));
        }
    }
}
