var sr = File.OpenText("input.txt");

var outcomes = new Dictionary<string, int>
{
    {"A X", 3 + 0}, // Rock <=> Scissor to Lose
    {"A Y", 1 + 3}, // Rock <=> Rock to Draw
    {"A Z", 2 + 6}, // Rock <=> Paper to Win
    
    {"B X", 1 + 0}, // Paper <=> Rock to Lose
    {"B Y", 2 + 3}, // Paper <=> Paper to Draw
    {"B Z", 3 + 6}, // Paper <=> Scissor to Win
    
    {"C X", 2 + 0}, // Scissor <=> Paper to Lose
    {"C Y", 3 + 3}, // Scissor <=> Scissor to Draw
    {"C Z", 1 + 6}, // Scissor <=> Rock to Win
};

var total = 0;

while (!sr.EndOfStream)
{
    var line = sr.ReadLine();
    total += outcomes[line!];
}

Console.WriteLine($"🎄 {total} 🎄");