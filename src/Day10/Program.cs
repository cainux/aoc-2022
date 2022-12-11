var lines = File.ReadAllLines("input.txt");

Part01();
Part02();

void Part01()
{
    var cpu = new CPU();
    var i = 0;

    for (var cycle = 1; cycle <= 220; cycle++)
    {
        if (cpu.AwaitingInstruction)
            cpu.LoadInstruction(lines[i++]);

        cpu.CycleWithOutput(cycle);
    }

    Console.WriteLine($"🎄 {cpu.Signal} 🎄");
}

void Part02()
{
    const int cols = 40;
    const int rows = 6;
    var i = 0;
    var cpu = new CPU();

    for (var r = 0; r < rows; r++)
    {
        for (var c = 0; c < cols; c++)
        {
            Console.Write(Math.Abs(c - cpu.X) < 2 ? '#' : '.');
            
            if (cpu.AwaitingInstruction)
                cpu.LoadInstruction(lines[i++]);

            cpu.Cycle();
        }
        Console.WriteLine();
    }
}

internal class CPU
{
    public int X { get; private set; } = 1;
    public int Signal { get; private set; }
    public bool AwaitingInstruction { get; private set; } = true;

    private readonly int[] _outputCycles = { 20, 60, 100, 140, 180, 220 };
    private string _instruction = string.Empty;
    private int _timer;
    private int _xd;

    public void LoadInstruction(string line)
    {
        var instruction = line.Split(' ');

        switch (instruction[0])
        {
            case "noop":
                AwaitingInstruction = false;
                _instruction = "noop";
                _timer = 1;
                break;
            
            case "addx":
                AwaitingInstruction = false;
                _instruction = "addx";
                _timer = 2;
                _xd = int.Parse(instruction[1]);
                break;
        }
    }

    public void CycleWithOutput(int cycle)
    {
        if (_outputCycles.Contains(cycle))
        {
            Console.WriteLine($"({cycle:000}) X: {X:000} Signal: {cycle * X:0000}");
            Signal += cycle * X;
        }
        Cycle();
    }

    public void Cycle()
    {
        if (_timer > 0) _timer--;
        if (_timer != 0) return;

        if (_instruction == "addx")
            X += _xd;

        _instruction = string.Empty;
        _xd = 0;
        AwaitingInstruction = true;
    }
}