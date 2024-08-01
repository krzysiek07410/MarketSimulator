namespace MarketSimulator;

public sealed class StateRecorder
{
    private readonly List<string> _records = new();
    private readonly string _path;

    public StateRecorder(string path)
    {
        _path = path.Replace(" ", "_").Replace("&", "n").ToLower();
    }

    public void AddRecord(params double[] args)
    {
        var record = _records.Count + ";" + string.Join(";", args);
        _records.Add(record);
    }

    public void SaveState()
    {
        var streamWriter = new StreamWriter(_path);
        foreach (var line in _records)
            streamWriter.WriteLine(line);
        streamWriter.Close();
    }
}