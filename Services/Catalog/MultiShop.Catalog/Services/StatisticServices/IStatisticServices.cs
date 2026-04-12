namespace MultiShop.Catalog.Services.StatisticServices
{
    public interface IStatisticServices
    {
        Task <long> GetCategoryCount();
        Task<long> GetProductCount();
        Task<long> GetBrandCount();
        Task<decimal> GetProductAvgPrice();
        Task<string> GetMaxPriceProductName();
        Task<string> GetMinPriceProductName();

    }
}
