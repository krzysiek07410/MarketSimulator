using MarketSimulator.Market;

namespace MarketSimulator.Interfaces;

public interface IVisitor
{
    public void Visit(Seller seller);
}