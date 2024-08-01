using MarketSimulator.Interfaces;

namespace MarketSimulator.Market;

public sealed class CentralBank : IMarketElement, IObservable
{
    public static CentralBank? Instance { get; private set; }

    public static Dictionary<uint, double> IncomeHistory { get; } = new()
    {
        { 0, 0 }
    };

    public double Inflation
    {
        get => _inflation;
        set
        {
            _inflation = value;
            NotifyObservers();
            MarketState.InflationHistory.Add(MarketState.InflationHistory.Count, _inflation);
            //Console.WriteLine($"Inflation: {_inflation}");
        }
    }

    private double _inflation;
    private readonly List<IObserver> _observers;

    public CentralBank()
    {
        if (Instance != null)
            throw new InvalidOperationException("Cannot create another instance of singleton class");
        Instance = this;
        _observers = new List<IObserver>();
    }

    public void PerformActions() => CalculateInflation();

    private static int IsInflationGrowing()
    {
        if (IncomeHistory.Count < 2)
            return 1;

        var lastIncome = IncomeHistory[IncomeHistory.Keys.Max()];
        var previousIncome = IncomeHistory[IncomeHistory.Keys.Max() - 1];

        if (Math.Abs(lastIncome - previousIncome) < double.Epsilon)
            return 0;
        if (lastIncome < previousIncome)
            return -1;
        if (lastIncome > previousIncome)
            return 1;
        return 0;
    }

    private void CalculateInflation()
    {
        var random = new Random();
        var newInflation = random.NextDouble() * 0.01;

        int multiplier = IsInflationGrowing();
        Inflation = newInflation * multiplier;
    }

    public void RegisterObserver(IObserver observer) => _observers.Add(observer);

    public void UnregisterObserver(IObserver observer) => _observers.Remove(observer);

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
            observer.Update();
    }
}