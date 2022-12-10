var lines = File.ReadAllLines("input.txt");

var height = lines.Length;
var width = lines[0].Length;

// Part 1
var visibleTrees = height * 2 + width * 2 - 4;

for (var y = 1; y < height - 1; y++)
for (var x = 1; x < width - 1; x++)
{
    var tHeight = Convert.ToInt16(lines[y][x].ToString());
    visibleTrees += IsPositionVisible(x, y, tHeight) ? 1 : 0;
}

Console.WriteLine($"🎄 {visibleTrees} 🎄");

bool IsPositionVisible(int tX, int tY, int tHeight)
{
    var xLine = lines[tY];
    
    if (Visible(tHeight, xLine.Substring(0, tX)) || Visible(tHeight, xLine.Substring(tX + 1)))
        return true;

    var sb = new System.Text.StringBuilder();
    for (var y = 0; y < height; y++)
        sb.Append(lines[y][tX]);
    var yLine = sb.ToString();
    
    if (Visible(tHeight, yLine.Substring(0, tY)) || Visible(tHeight, yLine.Substring(tY + 1)))
        return true;

    return false;
}

bool Visible(int tHeight, string line)
{
    var trees = line.Select(c => Convert.ToInt16(c.ToString()));
    return trees.All(t => t < tHeight);
}

// Part 2
var bestScore = 0;

for (var y = 1; y < height - 1; y++)
for (var x = 1; x < width - 1; x++)
{
    var score = CalculateScenicScore(x, y, Convert.ToInt16(lines[y][x].ToString()));
    if (score > bestScore)
        bestScore = score;
}

Console.WriteLine($"🎄 {bestScore} 🎄");

int CalculateScenicScore(int tX, int tY, int tHeight)
{
    var xLine = lines[tY];
    
    var sb = new System.Text.StringBuilder();
    for (var y = 0; y < height; y++)
        sb.Append(lines[y][tX]);

    var yLine = sb.ToString();

    var up = new string(yLine.Substring(0, tY).Reverse().ToArray());
    var left = new string(xLine.Substring(0, tX).Reverse().ToArray());
    var down = yLine.Substring(tY + 1);
    var right = xLine.Substring(tX + 1);

    var scoreUp = Score(tHeight, up);
    var scoreLeft = Score(tHeight, left);
    var scoreDown = Score(tHeight, down);
    var scoreRight = Score(tHeight, right);

    return scoreUp * scoreLeft * scoreDown * scoreRight;
}

int Score(int tHeight, string line)
{
    var trees = line.Select(c => Convert.ToInt16(c.ToString())).ToArray();
    var score = 0;

    foreach (var tree in trees)
    {
        score++;
        if (tree >= tHeight) break;
    }

    return score;
}