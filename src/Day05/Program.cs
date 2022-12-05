using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var stackNumbersLine = 0;

// Find the line that has just numbers and spaces
for (var i = 0; i < lines.Length; i++)
{
    if (Regex.IsMatch(lines[i], "^[\\s\\d]*$"))
    {
        stackNumbersLine = i;
        break;
    }
}

var stackNumbers = lines[stackNumbersLine].Split(' ', StringSplitOptions.RemoveEmptyEntries);

// We're only interested in the last number
var stacks = new Stack<char>[Convert.ToUInt16(stackNumbers[^1])];

// New up the Stacks
for (var i = 0; i < stacks.Length; i++)
    stacks[i] = new Stack<char>();

// Load the initial state
for (var i = stackNumbersLine - 1; i >= 0; i--)
{
    for (var s = 0; s < stacks.Length; s++)
    {
        var position = s * 4;
        var chunk = lines[i].Substring(position, 3).Replace("[", "").Replace("]", "");

        if (!string.IsNullOrWhiteSpace(chunk))
        {
            stacks[s].Push(chunk[0]);
        }
    }
}

// Part01();
Part02();

void Part01()
{
    for (var i = stackNumbersLine + 2; i < lines.Length; i++)
    {
        var line = lines[i];

        var instructions = line.Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(" ").Select(x => Convert.ToUInt16(x)).ToArray();
        var amount = instructions[0];
        var from = instructions[1];
        var to = instructions[2];

        for (var m = 0; m < amount; m++)
        {
            var crate = stacks[from - 1].Pop();
            stacks[to - 1].Push(crate);
        }
    }

    foreach (var stack in stacks)
    {
        Console.Write(stack.Peek());
    }
    
    Console.WriteLine();
}

void Part02()
{
    for (var i = stackNumbersLine + 2; i < lines.Length; i++)
    {
        var line = lines[i];

        var instructions = line.Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(" ").Select(x => Convert.ToUInt16(x)).ToArray();
        var amount = instructions[0];
        var from = instructions[1];
        var to = instructions[2];

        var tempStack = new Stack<char>();

        for (var m = 0; m < amount; m++)
        {
            var crate = stacks[from - 1].Pop();
            tempStack.Push(crate);
        }

        while (tempStack.Count > 0)
        {
            var crate = tempStack.Pop();
            stacks[to - 1].Push(crate);
        }
    }

    foreach (var stack in stacks)
    {
        Console.Write(stack.Peek());
    }
    
    Console.WriteLine();
}