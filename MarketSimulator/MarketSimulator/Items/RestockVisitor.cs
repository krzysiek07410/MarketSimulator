using MarketSimulator.Interfaces;
using MarketSimulator.Market;

namespace MarketSimulator.Items;

public sealed class RestockVisitor : IVisitor
{
    private bool _isInitial = true;

    public void Visit(Seller seller)
    {
        int neededProducts = _isInitial ? 1000 : 0;
        if (seller.Products.Count < 100 && !_isInitial)
        {
            bool shouldRestock = new Random().Next(10) < 7 || seller.Products.Count == 0;
            neededProducts = shouldRestock ? 100 : 0;
        }

        _isInitial = false;

        for (int i = 0; i < neededProducts; i++)
        {
            seller.AddProduct(new Product(
                Product.GetRandomProductType()
            ));
        }
    }
}