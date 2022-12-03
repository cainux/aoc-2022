var sr = File.OpenText("input.txt");
var Score = (char c) => c - (c < 97 ? 38 : 96);
var sb = new System.Text.StringBuilder();
var runningTotal = 0;

while (!sr.EndOfStream)
{
    for (var i = 0; i < 3; i++)
        sb.AppendJoin("", sr.ReadLine()!.Distinct());

    var common = sb.ToString().GroupBy(x => x)
        .Select(g => new
        {
            g.Key,
            Count = g.Count()
        })
        .Single(x => x.Count == 3);

    runningTotal += Score(common.Key);
    sb.Clear();
}

Console.WriteLine($"🎄 {runningTotal} 🎄");