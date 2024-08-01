namespace MarketSimulator.Market;

public sealed class MarketState
{
    public static Dictionary<int, double> InflationHistory { get; set; } = new()
    {
        { 0, 0 }
    };

    public static void SaveState()
    {
        var streamWriter = new StreamWriter("inflation.csv");
        foreach (var (key, value) in InflationHistory)
            streamWriter.WriteLine($"{key};{value}");
        streamWriter.Close();
    }
}