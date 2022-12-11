var lines = File.ReadAllLines("input.txt");
Console.Clear();
Part02();

void Part01()
{
    var rope = new Rope(2);
    var head = rope.Knots[0];
    var tail = rope.Knots[1];
    
    var type = head.GetType();

    int minX = 0, minY = 0, maxX = 0, maxY = 0;
    
    foreach (var line in lines)
    {
        var parts = line.Split(' ');
        var direction = parts[0];
        var steps = int.Parse(parts[1]);

        for (var i = 0; i < steps; i++)
        {
            type.GetMethod(direction)!.Invoke(head, null);
            // Render(rope);
            
            if (head.X < minX) minX = head.X;
            if (head.Y < minY) minY = head.Y;
            if (head.X > maxX) maxX = head.X;
            if (head.Y > maxY) maxY = head.Y;
            
            tail.Follow(head);
            // Render(rope);
        }
    }

    Render(rope);
    Console.WriteLine($"🎄 {tail.Visited.Count} {minX} {minY} {maxX} {maxY} 🎄");
    //   test:  0, 0, 6, 5
    // test-2: -12, -6, 16, 17
    //  input: -104, -274, 56, 6
}

void Part02()
{
    var rope = new Rope(10);
    var head = rope.Knots[0];
    var tail = rope.Knots[^1];
    
    var type = head.GetType();
    
    foreach (var line in lines)
    {
        var parts = line.Split(' ');
        var direction = parts[0];
        var steps = int.Parse(parts[1]);

        for (var i = 0; i < steps; i++)
        {
            type.GetMethod(direction)!.Invoke(head, null);
            // Render(rope);

            for (var j = 1; j < rope.Knots.Length; j++)
            {
                var knot = rope.Knots[j];
                knot.Follow(rope.Knots[j - 1]);
                // Render(rope);
            }
        }
    }

    Render(rope);
    Console.WriteLine($"🎄 {tail.Visited.Count} 🎄");
}

void Render(Rope rope, int minX = -12, int minY = -6, int maxX = 16, int maxY = 17)
{
    // Console.Clear();
    Console.SetCursorPosition(0, 0);

    for (var y = maxY - 1; y >= minY; y--)
    {
        for (var x = minX; x < maxX; x++)
        {
            var done = false;
            
            foreach (var knot in rope.Knots)
            {
                if (!knot.IsAt(x, y)) continue;
                Console.Write(knot.Name);
                done = true;
                break;
            }

            if (done) continue;
            Console.Write(rope.Knots[^1].Visited.Contains((x, y)) ? '#' : '.');
        }
        Console.WriteLine();
    }
    // Thread.Sleep(10);
    Console.WriteLine();
}

internal class Rope
{
    public Knot[] Knots { get; }

    public Rope(int knots)
    {
        Knots = new Knot[knots];

        for (var i = 0; i < knots; i++)
        {
            Knots[i] = new Knot { Name = i.ToString() };
        }

        Knots[0].Name = "H";
        Knots[^1].Name = "T";
    }
}

internal class Knot
{
    public string Name { get; set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public readonly HashSet<(int x, int y)> Visited = new();
    
    public void U() => Y++;
    public void D() => Y--;
    public void L() => X--;
    public void R() => X++;

    public Knot()
    {
        Visited.Add((0, 0));
    }
    
    public bool IsAt(int x, int y) => X == x && Y == y;
    public override string ToString() => $"{X},{Y}";

    public void Follow(Knot head)
    {
        var dx = X - head.X;
        var dy = Y - head.Y;
        
        // dx or dy need to be > 1
        if (Math.Abs(dx) < 2 && Math.Abs(dy) < 2) return;
        
        // Same row
        if (X == head.X && Y != head.Y)
        {
            if (dy > 0) D(); else U();
        }
        
        // Same column
        else if (Y == head.Y && X != head.X)
        {
            if (dx > 0) L(); else R();
        }
        
        // Different row and column
        else if (X != head.X && Y != head.Y)
        {
            /* tx < hx && ty < hy (-dx -dy)
             * .H.
             * T..
             */
            if (dx < 0 && dy < 0) { R(); U(); }
            
            /* tx > hx && ty < hy (+dx -dy)
             * H..
             * .T.
             */
            else if (dx > 0 && dy < 0) { L(); U(); }
            
            /* tx > hx && ty > hy (+dx +dy)
             * .T.
             * H..
             */
            else if (dx > 0 && dy > 0) { L(); D(); }
            
            /* tx < hx && ty > hy (-dx +dy)
             * .T.
             * ..H
             */
            else if (dx < 0 && dy > 0) { R(); D(); }
        }

        Visited.Add((X, Y));
    }
}