using MarketSimulator.Items;

namespace MarketSimulator.Market;

public sealed class Rule
{
    public ProductType Type { get; init; }
    public double MaxMoneyPerProduct { get; init; }
}