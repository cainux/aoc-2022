var lines = File.ReadAllLines("input.txt");

var rootDirectory = new Item
{
    Name = "/",
    Type = ItemType.Directory
};

var currentDirectory = rootDirectory;

foreach (var line in lines)
{
    var parts = line.Split(' ');

    switch (parts[0])
    {
        case "$":
        {
            if (parts[1] == "cd")
            {
                var targetPath = parts[2];
                currentDirectory = targetPath switch
                {
                    "/" => rootDirectory,
                    ".." => currentDirectory.Parent,
                    _ => currentDirectory.Children.Single(x => x.Name == targetPath)
                };
            }
            break;
        }
        case "dir":
        {
            var directory = parts[1];
            currentDirectory.Children.Add(new Item
            {
                Name = directory,
                Type = ItemType.Directory,
                Parent = currentDirectory
            });
            break;
        }
        default:
        {
            var fileSize = Convert.ToInt64(parts[0]);
            var fileName = parts[1];
            currentDirectory.Children.Add(new Item
            {
                Name = fileName,
                Size = fileSize,
                Type = ItemType.File
            });
            break;
        }
    }
}

// 🎄 Part 1 🎄
long result = 0;
Part01();
Console.WriteLine($"🎄 Part 1: {result} 🎄");

void Part01()
{
    Walk(rootDirectory, i =>
    {
        if (i.Type is not ItemType.Directory) return;
        var size = CalculateSize(i);
        if (size <= 100000)
            result += size;
    });
}

// 🎄 Part 2 🎄
const long diskSize = 70000000;
const long requiredSpace = 30000000;
var directories = new List<Item>();
Part02();

void Part02()
{
    Walk(rootDirectory, i =>
    {
        if (i.Type is not ItemType.Directory) return;
        var size = CalculateSize(i);
        i.Size = size;
        directories.Add(i);
    });
}

var directoryToDelete = directories
    .Skip(1)
    .OrderBy(x => x.Size)
    .First(x => x.Size + diskSize - rootDirectory.Size >= requiredSpace);

Console.WriteLine($"🎄 Part 2: {directoryToDelete.Name} ({directoryToDelete.Size}) 🎄");

// Utility Stuff
void Walk(Item item, Action<Item> action)
{
    action(item);
    foreach (var child in item.Children)
        Walk(child, action);
}

long CalculateSize(Item item)
{
    return item.Type switch
    {
        ItemType.File => item.Size,
        _ => item.Children.Sum(CalculateSize)
    };
}

internal enum ItemType { Directory, File }

internal class Item
{
    public string Name { get; init; } = default!;
    public ItemType Type { get; init; }
    public long Size { get; set; }
    public Item Parent { get; init; } = default!;
    public List<Item> Children { get; } = new();
}
