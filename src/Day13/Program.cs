using System.Text.Json;
const string inputFile = "input.txt";

Part01();
Part02();

void Part01()
{
    var sr = File.OpenText(inputFile);
    var pairNumber = 0;
    var result = 0;

    while (!sr.EndOfStream)
    {
        pairNumber++;
        var left = sr.ReadLine();
        var right = sr.ReadLine();
        sr.ReadLine();
        result += Compare(left, right) < 0 ? pairNumber : 0;
    }

    Console.WriteLine($"🎄 {result} 🎄");
}

void Part02()
{
    var dividers = new[] {"[[2]]", "[[6]]"};
    var packets = File.ReadAllLines(inputFile)
        .Where(x => x.Length > 0)
        .Concat(dividers)
        .ToList();
    packets.Sort(Compare);

    var result = (packets.IndexOf(dividers[0]) + 1) * (packets.IndexOf(dividers[1]) + 1);
    Console.WriteLine($"🎄 {result} 🎄");
}

static int Compare(string left, string right)
{
    return OuterCompare(JsonSerializer.Deserialize<JsonElement>(left), JsonSerializer.Deserialize<JsonElement>(right));
}

static int OuterCompare(JsonElement left, JsonElement right)
{
    return (left.ValueKind, right.ValueKind) switch
    {
        (JsonValueKind.Number, JsonValueKind.Number) => left.GetInt32() - right.GetInt32(),
        (JsonValueKind.Number, _) => InnerCompare(JsonSerializer.Deserialize<JsonElement>($"[{left.GetInt32()}]"), right),
        (_, JsonValueKind.Number) => InnerCompare(left, JsonSerializer.Deserialize<JsonElement>($"[{right.GetInt32()}]")),
        _ => InnerCompare(left, right)
    };
}

static int InnerCompare(JsonElement left, JsonElement right)
{
    int result;
    var le = left.EnumerateArray();
    var re = right.EnumerateArray();

    while (le.MoveNext() && re.MoveNext())
        if ((result = OuterCompare(le.Current, re.Current)) != 0)
            return result;

    return left.GetArrayLength() - right.GetArrayLength();
}