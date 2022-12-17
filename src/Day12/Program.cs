Part01();
Part02();

void Part01()
{
    var map = LoadMap();
    var goal = Locate('E', map);
    var start = Locate('S', map);
    var answer = GetShortestDistance(start, goal, map);

    Console.WriteLine($"🎄 {answer} 🎄");
}

void Part02()
{
    var map = LoadMap();
    var goal = Locate('E', map);
    var answer = map
        .Cast<Location>()
        .Where(x => x.Height == 'a')
        .Concat(new[] {Locate('S', map)})
        .Select(s => GetShortestDistance(s, goal, map))
        .Where(d => d > -1)
        .Min();

    Console.WriteLine($"🎄 {answer} 🎄");
}

static Location[,] LoadMap(string fileName = "input.txt")
{
    var lines = File.ReadAllLines(fileName);
    var width = lines[0].Length;
    var height = lines.Length;
    var map = new Location[width, height];

    for (var y = 0; y < height; y++)
    for (var x = 0; x < width; x++)
        map[x, y] = new Location(x, y, lines[y][x]);

    return map;
}

static int GetShortestDistance(Location start, Location goal, Location[,] grid)
{
    var distances = new Dictionary<Location, int?>();

    foreach (var location in grid)
        distances.Add(location, null);

    distances[start] = 0;

    var visited = new HashSet<Location>();
    var queue = new Queue<Location>();

    queue.Enqueue(start);

    while (queue.Any())
    {
        var current = queue.Dequeue();

        if (!visited.Add(current))
            continue;

        var neighbours = GetNextSteps(current, grid);

        foreach (var neighbour in neighbours)
        {
            distances[neighbour] = distances[current] + 1;
            queue.Enqueue(neighbour);
        }
    }

    return distances[goal] ?? -1;
}

static Location Locate(char c, Location[,] grid)
{
    foreach (var location in grid)
        if (location.Height == c)
            return location;

    throw new ApplicationException($"Could not locate '{c}'");
}

static IEnumerable<Location> GetNeighbours(Location current, Location[,] grid)
{
    var x = current.X;
    var y = current.Y;
    var mx = grid.GetUpperBound(0);
    var my = grid.GetUpperBound(1);

    if (y > 0) yield return grid[x, y - 1]; // Above
    if (y < my) yield return grid[x, y + 1]; // Below
    if (x > 0) yield return grid[x - 1, y]; // Left
    if (x < mx) yield return grid[x + 1, y]; // Right
}

static IEnumerable<Location> GetNextSteps(Location current, Location[,] grid)
{
    return GetNeighbours(current, grid)
        .Where(next => IsReachable(current.Height, next.Height));
}

static bool IsReachable(char current, char next)
{
    switch (current)
    {
        case 'S' when next is 'a':
        case 'z' when next is 'E':
            return true;
        default:
            return next - current <= 1;
    }
}

internal record Location(int X, int Y, char Height);