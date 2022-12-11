Part02();

void Part01()
{
    var monkeys = LoadMonkeys();
    const int rounds = 20;

    for (var r = 0; r < rounds; r++)
    {
        foreach (var monkey in monkeys)
        {
            monkey.Turn(monkeys);
        }
    }
    
    var result = monkeys.OrderByDescending(x => x.Inspections).Take(2).Select(x => x.Inspections).Aggregate((x, y) => x * y);
    Console.WriteLine($"🎄 {result} 🎄");
}

void Part02()
{
    var monkeys = LoadMonkeys();
    const int rounds = 10000;
    var outputRounds = new[] {1, 20, 1000, 2000, 3000, 4000, 5000, 6000, 8000, 9000, 10000};

    // Ended up looking at Reddit. The maths involved in this is beyond me.
    var modulo = monkeys.Select(x => x.TestDivisor).Aggregate((x, y) => x * y);
    
    for (var r = 0; r < rounds; r++)
    {
        foreach (var monkey in monkeys)
        {
            monkey.Turn(monkeys, modulo);
        }

        if (outputRounds.Contains(r + 1))
        {
            Console.WriteLine($"== After round {r + 1} ==");
            foreach (var monkey in monkeys)
                Console.WriteLine(monkey);
            Console.WriteLine();
        }
    }
    
    var result = monkeys.OrderByDescending(x => x.Inspections).Take(2).Select(x => x.Inspections).Aggregate((x, y) => x * y);
    Console.WriteLine($"🎄 {result} 🎄");
}

List<Monkey> LoadMonkeys()
{
    var lines = File.ReadAllLines("input.txt");

    var result = new List<Monkey>();
    Monkey? monkey = null;

    foreach (var line in lines)
    {
        if (string.IsNullOrEmpty(line))
        {
            monkey = null;
        }
        else if (line.StartsWith("Monkey"))
        {
            monkey = new Monkey(int.Parse(line[7..^1]));
            result.Add(monkey);
        }
        else if (line.StartsWith("  Starting items:"))
        {
            foreach (var item in line.Remove(0, 18).Split(',', StringSplitOptions.TrimEntries).Select(long.Parse))
            {
                monkey!.Items.Enqueue(item);
            }
        }
        else if (line.StartsWith("  Operation:"))
        {
            monkey!.Operation = line.Remove(0, 19);
        }
        else if (line.StartsWith("  Test:"))
        {
            monkey!.TestDivisor = int.Parse(line.Remove(0, 21));
        }
        else if (line.StartsWith("    If true:"))
        {
            monkey!.TrueTarget = int.Parse(line.Remove(0, 29));
        }
        else if (line.StartsWith("    If false:"))
        {
            monkey!.FalseTarget = int.Parse(line.Remove(0, 30));
        }
    }

    return result;
}

internal class Monkey
{
    private int MonkeyNumber { get; }
    public ulong Inspections { get; private set; }
    public Queue<long> Items { get; } = new();
    public string Operation { get; set; }
    public int TestDivisor { get; set; }
    public int TrueTarget { get; set; }
    public int FalseTarget { get; set; }

    public Monkey(int monkeyNumber)
    {
        MonkeyNumber = monkeyNumber;
    }

    public void Turn(List<Monkey> monkeys, int modulo = 0)
    {
        if (Items.Count == 0) return;

        while (Items.Count > 0)
        {
            var item = Items.Dequeue();
            
            // Inspect
            var expression = Operation.Replace("old", item.ToString());
            var newValue = Compute(expression);
            var boredValue = modulo == 0 ? newValue / 3 : newValue % modulo;
            
            // Test
            var testResult = boredValue % TestDivisor == 0;
            var targetMonkey = testResult ? TrueTarget : FalseTarget;
            
            // Throw
            monkeys[targetMonkey].Items.Enqueue(boredValue);
            
            // Increment
            Inspections++;
        }
    }

    public override string ToString()
    {
        return $"Monkey {MonkeyNumber} inspected items {Inspections} times.";
    }

    private long Compute(string expression)
    {
        var parts = expression.Split(' ');
        var left = long.Parse(parts[0]);
        var operation = parts[1];
        var right = long.Parse(parts[2]);

        return operation switch
        {
            "+" => left + right,
            "*" => left * right,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}