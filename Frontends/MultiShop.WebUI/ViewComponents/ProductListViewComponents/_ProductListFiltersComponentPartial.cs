using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Models.CategoryFilters;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListFiltersComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public _ProductListFiltersComponentPartial(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = new CategoryFilterModel { CategoryId = id };

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var category = await _categoryService.GetByIdCategoryAsync(id);
                    model.CategoryName = category?.CategoryName ?? string.Empty;
                }
                catch
                {
                    model.CategoryName = string.Empty;
                }
            }

            List<ResultProductWithCategoryDto> products = new();
            try
            {
                products = await _productService.GetProductsWithCategoryByCategoryIdAsync(id) ?? new();
            }
            catch
            {
                products = new();
            }

            model.TotalProductCount = products.Count;
            model.PriceFilter = BuildPriceFilter(products);
            model.Filters = BuildCategoryFilters(model.CategoryName);

            return View(model);
        }

        private static PriceFilterModel BuildPriceFilter(IReadOnlyCollection<ResultProductWithCategoryDto> products)
        {
            var result = new PriceFilterModel();
            if (products == null || products.Count == 0)
            {
                return result;
            }

            var min = products.Min(p => p.ProductPrice);
            var max = products.Max(p => p.ProductPrice);
            result.Min = min;
            result.Max = max;

            var range = max - min;
            const int bucketCount = 5;

            if (range <= 0)
            {
                result.Buckets.Add(new PriceBucketModel { From = min, To = max, Count = products.Count });
                return result;
            }

            var rawStep = range / bucketCount;
            var step = Math.Max(1m, Math.Ceiling(rawStep));

            for (int i = 0; i < bucketCount; i++)
            {
                var from = min + step * i;
                var to = i == bucketCount - 1 ? max : from + step;

                int count;
                if (i == bucketCount - 1)
                {
                    count = products.Count(p => p.ProductPrice >= from && p.ProductPrice <= to);
                }
                else
                {
                    var upper = to;
                    count = products.Count(p => p.ProductPrice >= from && p.ProductPrice < upper);
                }

                result.Buckets.Add(new PriceBucketModel { From = from, To = to, Count = count });

                if (to >= max)
                {
                    break;
                }
            }

            return result;
        }

        private static List<FilterGroupModel> BuildCategoryFilters(string categoryName)
        {
            var groups = new List<FilterGroupModel>();
            var name = (categoryName ?? string.Empty).ToLowerInvariant();

            bool Has(params string[] keys) => keys.Any(k => name.Contains(k));

            if (Has("ayakkab", "bot", "sneaker", "terlik", "sandalet"))
            {
                groups.Add(MakeGroup("size", "Numaraya Göre Filtrele",
                    "36", "37", "38", "39", "40", "41", "42", "43", "44", "45"));
                groups.Add(ColorGroup());
                groups.Add(GenderGroup());
            }
            else if (Has("giyim", "tişört", "tisort", "pantolon", "ceket", "elbise",
                         "eşofman", "esofman", "mont", "sweat", "gömlek", "gomlek",
                         "kazak", "şort", "sort", "etek", "tayt"))
            {
                groups.Add(MakeGroup("size", "Bedene Göre Filtrele",
                    "XS", "S", "M", "L", "XL", "XXL"));
                groups.Add(ColorGroup());
                groups.Add(GenderGroup());
            }
            else if (Has("telefon", "akıllı telefon", "akilli telefon", "cep"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Apple", "Samsung", "Xiaomi", "Huawei", "Oppo", "Honor"));
                groups.Add(MakeGroup("storage", "Depolamaya Göre Filtrele",
                    "64 GB", "128 GB", "256 GB", "512 GB", "1 TB"));
                groups.Add(ColorGroup());
            }
            else if (Has("bilgisayar", "laptop", "notebook", "masaüstü", "masaustu"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Apple", "Asus", "Lenovo", "HP", "Dell", "Acer", "MSI"));
                groups.Add(MakeGroup("ram", "RAM'e Göre Filtrele",
                    "8 GB", "16 GB", "32 GB", "64 GB"));
                groups.Add(MakeGroup("storage", "Depolamaya Göre Filtrele",
                    "256 GB SSD", "512 GB SSD", "1 TB SSD", "2 TB SSD"));
            }
            else if (Has("tablet"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Apple", "Samsung", "Lenovo", "Huawei"));
                groups.Add(MakeGroup("storage", "Depolamaya Göre Filtrele",
                    "32 GB", "64 GB", "128 GB", "256 GB", "512 GB"));
                groups.Add(ColorGroup());
            }
            else if (Has("saat"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Casio", "Fossil", "Seiko", "Citizen", "Apple", "Samsung"));
                groups.Add(MakeGroup("case", "Kasa Çapına Göre Filtrele",
                    "38 mm", "40 mm", "42 mm", "44 mm", "46 mm"));
                groups.Add(ColorGroup());
            }
            else if (Has("televizyon", "tv "))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Samsung", "LG", "Sony", "Philips", "Vestel", "Arçelik"));
                groups.Add(MakeGroup("screen", "Ekran Boyutuna Göre Filtrele",
                    "32\"", "43\"", "50\"", "55\"", "65\"", "75\""));
                groups.Add(MakeGroup("resolution", "Çözünürlüğe Göre Filtrele",
                    "HD", "Full HD", "4K UHD", "8K"));
            }
            else if (Has("kozmetik", "parfüm", "parfum", "makyaj", "bakım", "bakim"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Lancôme", "Estée Lauder", "MAC", "Chanel", "L'Oréal", "Maybelline"));
                groups.Add(MakeGroup("volume", "Hacme Göre Filtrele",
                    "30 ml", "50 ml", "100 ml", "200 ml"));
                groups.Add(MakeGroup("skin", "Cilt Tipine Göre Filtrele",
                    "Karma", "Yağlı", "Kuru", "Hassas", "Normal"));
            }
            else if (Has("aksesuar", "çanta", "canta", "cüzdan", "cuzdan",
                         "takı", "taki", "kemer", "şapka", "sapka"))
            {
                groups.Add(ColorGroup());
                groups.Add(MakeGroup("material", "Malzemeye Göre Filtrele",
                    "Deri", "Suni Deri", "Kumaş", "Metal"));
                groups.Add(GenderGroup());
            }
            else if (Has("ev", "mobilya", "mutfak", "yatak", "dekorasyon", "banyo"))
            {
                groups.Add(ColorGroup());
                groups.Add(MakeGroup("material", "Malzemeye Göre Filtrele",
                    "Ahşap", "Metal", "Cam", "Plastik", "Kumaş"));
            }
            else if (Has("spor", "fitness", "outdoor", "kamp"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Nike", "Adidas", "Puma", "Reebok", "Under Armour", "New Balance"));
                groups.Add(ColorGroup());
                groups.Add(GenderGroup());
            }
            else if (Has("kitap", "kırtasiye", "kirtasiye", "hobi"))
            {
                groups.Add(MakeGroup("genre", "Türe Göre Filtrele",
                    "Roman", "Çocuk", "Bilim", "Tarih", "Ekonomi", "Felsefe"));
                groups.Add(MakeGroup("publisher", "Yayınevine Göre Filtrele",
                    "Can", "İletişim", "Doğan", "İş Bankası", "YKY"));
            }
            else if (Has("bebek", "çocuk", "cocuk", "oyuncak"))
            {
                groups.Add(MakeGroup("age", "Yaş Aralığına Göre Filtrele",
                    "0-1 Yaş", "1-3 Yaş", "3-6 Yaş", "6-9 Yaş", "9-12 Yaş"));
                groups.Add(MakeGroup("gender", "Cinsiyete Göre Filtrele",
                    "Erkek Çocuk", "Kız Çocuk", "Unisex"));
                groups.Add(ColorGroup());
            }
            else if (Has("beyaz eşya", "beyaz esya", "buzdolab", "çamaşır", "camasir",
                         "bulaşık", "bulasik", "fırın", "firin"))
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Arçelik", "Bosch", "Siemens", "Vestel", "Beko", "Samsung", "LG"));
                groups.Add(MakeGroup("energy", "Enerji Sınıfına Göre Filtrele",
                    "A", "A+", "A++", "A+++"));
                groups.Add(ColorGroup());
            }
            else
            {
                groups.Add(MakeGroup("brand", "Markaya Göre Filtrele",
                    "Marka A", "Marka B", "Marka C", "Marka D"));
                groups.Add(ColorGroup());
            }

            return groups;
        }

        private static FilterGroupModel ColorGroup() =>
            MakeGroup("color", "Renge Göre Filtrele",
                "Siyah", "Beyaz", "Kırmızı", "Mavi", "Yeşil", "Sarı", "Gri");

        private static FilterGroupModel GenderGroup() =>
            MakeGroup("gender", "Cinsiyete Göre Filtrele",
                "Kadın", "Erkek", "Unisex");

        private static FilterGroupModel MakeGroup(string key, string title, params string[] options)
        {
            return new FilterGroupModel
            {
                Key = key,
                Title = title,
                Options = options.Select(o => new FilterOptionModel { Value = Slug(o), Label = o }).ToList()
            };
        }

        private static string Slug(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return new string(value.ToLowerInvariant()
                                   .Select(ch => char.IsLetterOrDigit(ch) ? ch : '-')
                                   .ToArray());
        }
    }
}
