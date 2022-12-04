var lines = File.ReadAllLines("input.txt");

Part1();
Part2();

void Part1()
{
    string Expand(string assignment)
    {
        var sb = new System.Text.StringBuilder();
        var pair = assignment.Split('-');

        for (var i = Convert.ToUInt16(pair[0]); i <= Convert.ToUInt16(pair[1]); i++)
            sb.Append($".{i}.");

        return sb.ToString();
    }

    bool ContainsOther(string[] assignments) => assignments[0].Contains(assignments[1]) || assignments[1].Contains(assignments[0]);

    var count = lines
        .Select(line => line.Split(','))
        .Select(assignments =>  new[] { Expand(assignments[0]), Expand(assignments[1])})
        .Count(ContainsOther);
    
    Console.WriteLine($"🎄 {count} 🎄");
}

void Part2()
{
    bool Overlaps(string line)
    {
        var assignments = line.Split(',').Select(p => p.Split('-').Select(q => Convert.ToUInt16(q))).ToArray();
        var x = assignments[0].ToArray(); var y = assignments[1].ToArray();
        return x[0] <= y[1] && x[1] >= y[0];
    }
    
    Console.WriteLine($"🎄 {lines.Count(Overlaps)} 🎄");
}