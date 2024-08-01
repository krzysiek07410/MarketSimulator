using MarketSimulator.Interfaces;
using MarketSimulator.Items;

namespace MarketSimulator.Market;

public sealed class Seller : IObserver, IMarketElement
{
    public List<Product> Products { get; } = new();
    private readonly RestockVisitor _restockVisitor = new();
    private readonly StateRecorder _stateRecorder;

    public Seller(string name)
    {
        _stateRecorder = new StateRecorder($"sellers/{name}.csv");
    }

    public void PerformActions()
    {
        _restockVisitor.Visit(this);

        foreach (var buyer in Market.Buyers)
        {
            foreach (var product in Products)
            {
                if (!buyer.CanBuyProduct(product)) continue;
                SellProduct(product);
                buyer.BuyProduct(product);
                break;
            }

            buyer.UpdateDesiredProducts();
        }

        UpdateState();
    }

    public void SellProduct(Product product)
    {
        Products.Remove(product);
        CentralBank.IncomeHistory.Add(CentralBank.IncomeHistory.Keys.Max() + 1, product.Price);
    }

    public void AddProduct(Product product) => Products.Add(product);

    public void Update()
    {
        foreach (var product in Products)
        {
            if (MarketState.InflationHistory.Count <= 2) continue;
            product.Update();
        }
    }

    private void UpdateState()
    {
        if (Products.Count == 0)
        {
            _stateRecorder.AddRecord(0, 0, 0);
            return;
        }

        var maxPrice = Products.Max(p => p.Price);
        var minPrice = Products.Min(p => p.Price);
        _stateRecorder.AddRecord(Products.Count, maxPrice, minPrice);
    }

    public void SaveState() => _stateRecorder.SaveState();
}