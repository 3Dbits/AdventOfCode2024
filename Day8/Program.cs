string filePath = "input.txt";
char[][] grid = (await File.ReadAllLinesAsync(filePath))
    .Select(line => line.ToCharArray())
    .ToArray();

int gridLegth = grid.Length;
int gridWidth = grid[0].Length;

var charLocations = ExtractCharLocations(grid);
charLocations.Remove('.');

SortedSet<Point> partOneResult = [];
SortedSet<Point> partTwoResult = [];

foreach (var (key, value) in charLocations)
{
    var uniqueCombinations = GetUniqueCombinations(value);
    foreach (var (first, second) in uniqueCombinations)
    {
        var (antiNode1, antiNode2) = FindDirectionalPoints(first, second);
        if (!IsCordinatesOutOfBounds(antiNode1))
        {
            partOneResult.Add(antiNode1);
        }
        if (!IsCordinatesOutOfBounds(antiNode2))
        {
            partOneResult.Add(antiNode2);
        }
    }
}

foreach (var (key, value) in charLocations)
{
    var uniqueCombinations = GetUniqueCombinations(value);
    foreach (var (first, second) in uniqueCombinations)
    {
        var listOfAntiNodes = FindAllDirectionalPoints(first, second);
        partTwoResult.UnionWith(listOfAntiNodes);
    }

    partTwoResult.UnionWith(value);
}

Console.WriteLine(partOneResult.Count);
Console.WriteLine(partTwoResult.Count);
PrintGrid(grid, [.. partTwoResult]);

bool IsCordinatesOutOfBounds(Point point)
{
    return point.X < 0 || point.X >= gridLegth || point.Y < 0 || point.Y >= gridWidth;
}

(Point, Point) FindDirectionalPoints(Point p1, Point p2)
{
    int dx = p2.X - p1.X;
    int dy = p2.Y - p1.Y;

    Point newPoint1 = new(p1.X - dx, p1.Y - dy);

    Point newPoint2 = new(p2.X + dx, p2.Y + dy);

    return (newPoint1, newPoint2);
}

List<Point> FindAllDirectionalPoints(Point p1, Point p2)
{
    List<Point> points = [];
    int dx = p2.X - p1.X;
    int dy = p2.Y - p1.Y;

    Point newPoint1 = p1;
    Point newPoint2 = p2;

    while (true)
    {
        newPoint1 = new Point(newPoint1.X - dx, newPoint1.Y - dy);
        newPoint2 = new Point(newPoint2.X + dx, newPoint2.Y + dy);

        bool isNewPoint1InBounds = !IsCordinatesOutOfBounds(newPoint1);
        bool isNewPoint2InBounds = !IsCordinatesOutOfBounds(newPoint2);

        if (isNewPoint1InBounds)
        {
            points.Add(newPoint1);
        }
        if (isNewPoint2InBounds)
        {
            points.Add(newPoint2);
        }

        if (!isNewPoint1InBounds && !isNewPoint2InBounds)
        {
            break;
        }
    }

    return points;
}

List<(Point, Point)> GetUniqueCombinations(List<Point> points)
{
    var uniqueCombinations = new List<(Point, Point)>();

    for (int i = 0; i < points.Count; i++)
    {
        for (int j = i + 1; j < points.Count; j++)
        {
            uniqueCombinations.Add((points[i], points[j]));
        }
    }

    return uniqueCombinations;
}

Dictionary<char, List<Point>> ExtractCharLocations(char[][] grid)
{
    var locations = new Dictionary<char, List<Point>>();

    for (int x = 0; x < gridLegth; x++)
    {
        for (int y = 0; y < gridWidth; y++)
        {
            char current = grid[x][y];

            if (!locations.TryGetValue(current, out List<Point>? value))
            {
                value = [];
                locations[current] = value;
            }

            value.Add(new Point(x, y));
        }
    }

    return locations;
}

static void PrintGrid(char[][] grid, List<Point> points)
{
    foreach (var point in points)
    {
        grid[point.X][point.Y] = '#';
    }

    for (int y = 0; y < grid.Length; y++)
    {
        for (int x = 0; x < grid[y].Length; x++)
        {
            Console.Write(grid[y][x]);
        }
        Console.WriteLine();
    }
}

record Point(int X, int Y) : IComparable<Point>
{
    public int CompareTo(Point? other)
    {
        int xComparison = X.CompareTo(other?.X);
        if (xComparison != 0)
        {
            return xComparison;
        }

        return Y.CompareTo(other?.Y);
    }
}