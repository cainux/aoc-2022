var sr = File.OpenText("input.txt");
var elves = new[] {0, 0, 0};
var runningValue = 0;

while (!sr.EndOfStream)
{
    var line = sr.ReadLine();

    if (!string.IsNullOrEmpty(line))
    {
        runningValue += Convert.ToInt32(line);
    }
    else
    {
        if (runningValue > elves[0])
        {
            elves[0] = runningValue;
        }
        Array.Sort(elves);
        runningValue = 0;
    }
}

Console.WriteLine($"🎄 {elves.Sum()} 🎄");