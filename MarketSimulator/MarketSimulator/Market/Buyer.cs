using MarketSimulator.Interfaces;
using MarketSimulator.Items;

namespace MarketSimulator.Market;

public sealed class Buyer : IObserver, IMarketElement
{
    public double Money { get; set; }
    public double Income { get; set; }
    private List<Product> OwnedProducts { get; } = new();

    public List<ProductType> DesiredProducts { get; } = new()
    {
        ProductType.Other,
        ProductType.BasicNecessities,
        ProductType.LuxuryGoods
    };

    private List<Rule> Rules { get; } = new(
        new List<Rule>
        {
            new()
            {
                Type = ProductType.BasicNecessities,
                MaxMoneyPerProduct = .33,
            },
            new()
            {
                Type = ProductType.LuxuryGoods,
                MaxMoneyPerProduct = .05,
            },
            new()
            {
                Type = ProductType.Other,
                MaxMoneyPerProduct = .25,
            }
        }
    );

    private readonly StateRecorder _stateRecorder;
    private readonly double _baseIncome;
    private double _moneySpent;

    public Buyer(string name)
    {
        Money = 1000;
        Income = new Random().Next(50, 100) * 10;
        _baseIncome = Income;
        _stateRecorder = new StateRecorder($"buyers/{name}.csv");
    }

    public void PerformActions()
    {
        UpdateProducts();
        foreach (var seller in Market.Sellers)
        {
            foreach (var product in seller.Products.ToList())
            {
                if (!CanBuyProduct(product)) continue;
                BuyProduct(product);
                seller.SellProduct(product);
                break;
            }

            UpdateDesiredProducts();
        }

        EarnMoney();
        _stateRecorder.AddRecord(Money, OwnedProducts.Count, _moneySpent, _baseIncome, Income);
        _moneySpent = 0;
    }

    public bool CanBuyProduct(Product product)
    {
        foreach (var rule in Rules)
        {
            var maxMoneyPerProduct = rule.MaxMoneyPerProduct * Money;
            if (product.Type == rule.Type && product.Price > maxMoneyPerProduct)
                return false;
        }

        return Money > product.Price && DesiredProducts.Contains(product.Type);
    }

    public void BuyProduct(Product product)
    {
        Money -= product.Price;
        _moneySpent += product.Price;
        OwnedProducts.Add(product);
        UpdateDesiredProducts();
    }

    public void Update()
    {
        //Console.WriteLine($"Buyer {Name} received information about inflation change.");
    }

    private void UpdateProducts()
    {
        foreach (var product in OwnedProducts.ToList())
        {
            product.DecreaseExpirationDate();
            if (product.ExpirationDate <= 0)
                OwnedProducts.Remove(product);
        }
    }

    public void UpdateDesiredProducts()
    {
        DesiredProducts.Clear();

        var luxuryGoods = OwnedProducts.Count(product => product.Type == ProductType.LuxuryGoods);
        var basicNecessities = OwnedProducts.Count(product => product.Type == ProductType.BasicNecessities);
        var other = OwnedProducts.Count(product => product.Type == ProductType.Other);

        if (luxuryGoods == 1) DesiredProducts.Add(ProductType.LuxuryGoods);
        if (basicNecessities < 9) DesiredProducts.Add(ProductType.BasicNecessities);
        if (other < 5) DesiredProducts.Add(ProductType.Other);
    }

    private void EarnMoney()
    {
        Income += Income * CentralBank.Instance!.Inflation;
        Money += Income;
    }

    public void SaveState() => _stateRecorder.SaveState();
}