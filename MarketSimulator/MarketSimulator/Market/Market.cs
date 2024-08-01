namespace MarketSimulator.Market;

public sealed class Market
{
    private readonly CentralBank _centralBank = new();
    public static List<Seller> Sellers { get; } = new();
    public static List<Buyer> Buyers { get; } = new();

    public void PerformActions()
    {
        _centralBank.PerformActions();

        foreach (var seller in Sellers)
        {
            seller.PerformActions();
        }

        foreach (var buyer in Buyers)
        {
            buyer.PerformActions();
        }
    }

    public void AddSeller(Seller seller)
    {
        Sellers.Add(seller);
        _centralBank.RegisterObserver(seller);
    }

    public void AddBuyer(Buyer buyer)
    {
        Buyers.Add(buyer);
        _centralBank.RegisterObserver(buyer);
    }

    public void AddSellers(IEnumerable<Seller> sellers)
    {
        foreach (var seller in sellers) AddSeller(seller);
    }

    public void AddBuyers(IEnumerable<Buyer> buyers)
    {
        foreach (var buyer in buyers) AddBuyer(buyer);
    }
}