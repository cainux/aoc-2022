var lines = File.ReadAllLines("input.txt");

Console.WriteLine("Part 1:");
foreach (var line in lines) Console.WriteLine($"🎄 {LocateMarker(line, 4)} 🎄");
Console.WriteLine("Part 2:");
foreach (var line in lines) Console.WriteLine($"🎄 {LocateMarker(line, 14)} 🎄");

int LocateMarker(string line, int markerLength)
{
    var lineSpan = line.AsSpan();
    
    for (var i = 0; i < lineSpan.Length - markerLength - 1; i++)
        if (lineSpan.Slice(i, markerLength).ToArray().Distinct().Count() == markerLength)
            return i + markerLength;

    return 0;
}