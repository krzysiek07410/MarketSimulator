using MarketSimulator.Market;

namespace MarketSimulator.Interfaces;

public interface IMarketVisitor
{
    void VisitSeller(Seller seller);
    void VisitBuyer(Buyer buyer);
}