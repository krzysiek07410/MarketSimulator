using MarketSimulator.Market;

namespace MarketSimulator.Items;

public sealed class Product
{
    public ProductType Type { get; }
    public double Price { get; private set; }
    public int ExpirationDate { get; private set; }

    public Product(ProductType type)
    {
        Type = type;
        Price = CalculatePrice(type);
        ExpirationDate = GetExpirationDate(type);
    }

    private static double CalculatePrice(ProductType type)
    {
        var random = new Random();
        var tax = GetTaxRate(type);
        var typeMultiplier = GetPriceMultiplier(type);
        var manufacturingCost = random.Next(2, 50);
        var profit = random.Next(10, 200);
        return (manufacturingCost + profit) * tax * typeMultiplier;
    }

    private static double GetTaxRate(ProductType type)
    {
        return type switch
        {
            ProductType.BasicNecessities => 1.085,
            ProductType.LuxuryGoods => 1.23,
            _ => 1.23
        };
    }

    public void Update()
    {
        if (MarketState.InflationHistory.Count <= 2) return;
        Price += Price * CentralBank.Instance!.Inflation;
    }

    public void DecreaseExpirationDate() => ExpirationDate--;

    public static ProductType GetRandomProductType()
    {
        var random = new Random();
        return random.Next(0, 3) switch
        {
            0 => ProductType.BasicNecessities,
            1 => ProductType.LuxuryGoods,
            _ => ProductType.Other
        };
    }

    private static double GetPriceMultiplier(ProductType type)
    {
        return type switch
        {
            ProductType.BasicNecessities => 0.8,
            ProductType.LuxuryGoods => 1.5,
            _ => 1
        };
    }

    private static int GetExpirationDate(ProductType type)
    {
        return type switch
        {
            ProductType.BasicNecessities => 2,
            ProductType.LuxuryGoods => 8,
            ProductType.Other => 4,
            _ => 4
        };
    }

    public override string ToString() => Type.ToString();
}