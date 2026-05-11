using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            var basket = await GetBasket() ?? new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            basket.BasketItems ??= new List<BasketItemDto>();

            var existing = basket.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);
            if (existing != null)
            {
                existing.Quantity += basketItemDto.Quantity > 0 ? basketItemDto.Quantity : 1;
            }
            else
            {
                if (basketItemDto.Quantity <= 0) basketItemDto.Quantity = 1;
                basket.BasketItems.Add(basketItemDto);
            }

            await SaveBasket(basket);
        }

        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BasketTotalDto> GetBasket()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("baskets");
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
                }

                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonData) || jsonData == "null")
                {
                    return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
                }

                var values = System.Text.Json.JsonSerializer.Deserialize<BasketTotalDto>(jsonData,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (values == null)
                {
                    return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
                }

                values.BasketItems ??= new List<BasketItemDto>();
                return values;
            }
            catch
            {
                return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            }
        }

        public async Task<(bool removed, int newQuantity)> UpdateBasketItemQuantity(string productId, int delta)
        {
            if (string.IsNullOrWhiteSpace(productId)) return (false, 0);

            var basket = await GetBasket();
            basket.BasketItems ??= new List<BasketItemDto>();

            var item = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return (false, 0);

            var newQty = item.Quantity + delta;
            if (newQty <= 0)
            {
                basket.BasketItems.Remove(item);
                await SaveBasket(basket);
                return (true, 0);
            }

            item.Quantity = newQty;
            await SaveBasket(basket);
            return (false, newQty);
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            var basket = await GetBasket();
            if (basket?.BasketItems == null || basket.BasketItems.Count == 0)
            {
                return false;
            }

            var deletedItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (deletedItem == null)
            {
                return false;
            }

            basket.BasketItems.Remove(deletedItem);
            await SaveBasket(basket);
            return true;
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            basketTotalDto ??= new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            basketTotalDto.BasketItems ??= new List<BasketItemDto>();
            var response = await _httpClient.PostAsJsonAsync<BasketTotalDto>("baskets", basketTotalDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
