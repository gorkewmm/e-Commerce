namespace MultiShop.WebUI.Services.FavoriteServices
{
    public class FavoriteService : IFavoriteService
    {
        private const string CookieName = "MultiShopFavorites";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoriteService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<string> GetFavoriteIds()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx == null) return new List<string>();

            if (!ctx.Request.Cookies.TryGetValue(CookieName, out var raw) || string.IsNullOrWhiteSpace(raw))
            {
                return new List<string>();
            }

            return raw.Split('|', StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => x.Trim())
                      .Where(x => !string.IsNullOrWhiteSpace(x))
                      .Distinct()
                      .ToList();
        }

        public bool IsFavorite(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) return false;
            return GetFavoriteIds().Contains(productId);
        }

        public void AddFavorite(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) return;
            var ids = GetFavoriteIds();
            if (!ids.Contains(productId))
            {
                ids.Add(productId);
                Save(ids);
            }
        }

        public void RemoveFavorite(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) return;
            var ids = GetFavoriteIds();
            if (ids.Remove(productId))
            {
                Save(ids);
            }
        }

        public void Clear()
        {
            var ctx = _httpContextAccessor.HttpContext;
            ctx?.Response.Cookies.Delete(CookieName);
        }

        public int Count() => GetFavoriteIds().Count;

        private void Save(List<string> ids)
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx == null) return;

            var value = string.Join('|', ids);
            ctx.Response.Cookies.Append(CookieName, value, new CookieOptions
            {
                HttpOnly = false,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Secure = ctx.Request.IsHttps
            });
        }
    }
}
