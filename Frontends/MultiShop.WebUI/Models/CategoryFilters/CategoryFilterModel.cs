namespace MultiShop.WebUI.Models.CategoryFilters
{
    public class CategoryFilterModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalProductCount { get; set; }
        public PriceFilterModel PriceFilter { get; set; } = new();
        public List<FilterGroupModel> Filters { get; set; } = new();
    }

    public class PriceFilterModel
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public List<PriceBucketModel> Buckets { get; set; } = new();
    }

    public class PriceBucketModel
    {
        public decimal From { get; set; }
        public decimal To { get; set; }
        public int Count { get; set; }
    }

    public class FilterGroupModel
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public List<FilterOptionModel> Options { get; set; } = new();
    }

    public class FilterOptionModel
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
