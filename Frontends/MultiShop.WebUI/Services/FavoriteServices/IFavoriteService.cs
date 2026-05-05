namespace MultiShop.WebUI.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        List<string> GetFavoriteIds();
        bool IsFavorite(string productId);
        void AddFavorite(string productId);
        void RemoveFavorite(string productId);
        void Clear();
        int Count();
    }
}
