using MarketSimulator.Market;

namespace MarketSimulator;

internal sealed class Program
{
    public static void Main()
    {
        var market = new Market.Market();

        var sellers = new List<Seller>
        {
            new("Tom LLC"),
            new("Jerry LLC"),
            new("Spike GmbH"),
            new("Tyke GmbH"),
            new("Butch AG"),
            new("Nibbles AG"),
            new("Lightning Ltd"),
            new("Topsy Ltd"),
            new("Mammy S.A."),
            new("Quacker S.A."),
            new("Droopy Inc."),
            new("Screwy Inc."),
            new("Spike & Tyke"),
            new("Tom & Jerry"),
            new("Droopy & Quacker"),
            new("Screwy & Lightning"),
        };

        var buyers = new List<Buyer>
        {
            new("John Doe"),
            new("Jane Doe"),
            new("Mark Smith"),
            new("Anna Smith"),
            new("Michael Johnson"),
            new("Emma Johnson"),
            new("Joshua Williams"),
            new("Olivia Williams"),
            new("Daniel Brown"),
            new("Sophia Brown"),
            new("Matthew Jones"),
            new("Isabella Jones"),
            new("Christopher Miller"),
            new("Emily Miller"),
            new("Andrew Davis"),
            new("Abigail Davis"),
        };

        market.AddSellers(sellers);
        market.AddBuyers(buyers);

        for (int i = 0; i < 30; i++)
            market.PerformActions();

        foreach (var buyer in buyers)
            buyer.SaveState();

        foreach (var seller in sellers)
            seller.SaveState();

        MarketState.SaveState();
    }
}