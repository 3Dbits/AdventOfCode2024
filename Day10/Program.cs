string filePath = "input.txt";
int[][] grid = (await File.ReadAllLinesAsync(filePath))
    .Select(line => line.Select(c => int.Parse(c.ToString())).ToArray())
    .ToArray();

int gridLegth = grid.Length;
int gridWidth = grid[0].Length;

(int x, int y)[] directions =
[
    (1, 0), (0, 1), (-1, 0), (0, -1)
];

List<Point> startingCoordinates = [];
HashSet<Point> uniqEndPoints = [];
int resultCount = default;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[i].Length; j++)
    {
        if (grid[i][j] == 0)
        {
            startingCoordinates.Add(new Point(i, j));
        }
    }
}

// Part 1
foreach (var coordinate in startingCoordinates)
{
    IsValidPath(coordinate, new Point(-2, -2), false);
    resultCount += uniqEndPoints.Count;
    uniqEndPoints.Clear();
}

Console.WriteLine(resultCount);

// Part 2
resultCount = default;
foreach (var coordinate in startingCoordinates)
{
    IsValidPath(coordinate, new Point(-2, -2), true);
}

Console.WriteLine(resultCount);


void IsValidPath(Point startingPoint, Point lastStep, bool secondPart, int position = 0)
{

    if (IsCordinatesOutOfBounds(startingPoint.X, startingPoint.Y))
    {
        return;
    }
    if (grid[startingPoint.X][startingPoint.Y] != position)
    {
        return;
    }
    if (position == 9  && grid[startingPoint.X][startingPoint.Y] == 9)
    {
        if (secondPart)
        {
            resultCount++;
        }
        else
        {
        uniqEndPoints.Add(startingPoint);

        }
        return;
    }

    var down = new Point(startingPoint.X + directions[0].x, startingPoint.Y + directions[0].y);
    var right = new Point(startingPoint.X + directions[1].x, startingPoint.Y + directions[1].y);
    var up = new Point(startingPoint.X + directions[2].x, startingPoint.Y + directions[2].y);
    var left = new Point(startingPoint.X + directions[3].x, startingPoint.Y + directions[3].y);

    if (!IsCordinatesOutOfBounds(up.X, up.Y) && up != lastStep)
    {
        IsValidPath(up, startingPoint, secondPart, position + 1);
    }
    if (!IsCordinatesOutOfBounds(right.X, right.Y) && right != lastStep)
    {
        IsValidPath(right, startingPoint, secondPart, position + 1);
    }
    if (!IsCordinatesOutOfBounds(down.X, down.Y) && down != lastStep)
    {
        IsValidPath(down, startingPoint, secondPart, position + 1);
    }
    if (!IsCordinatesOutOfBounds(left.X, left.Y) && left != lastStep)
    {
        IsValidPath(left, startingPoint, secondPart, position + 1);
    }
}

bool IsCordinatesOutOfBounds(int x, int y)
{
    return x < 0 || x >= gridLegth || y < 0 || y >= gridWidth;
}

public struct Point(int x, int y) : IEquatable<Point>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public bool Equals(Point other) => X == other.X && Y == other.Y;

    public override bool Equals(object obj) =>
        obj is Point other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    // Optional: Implement == and != operators
    public static bool operator ==(Point left, Point right) => left.Equals(right);
    public static bool operator !=(Point left, Point right) => !(left == right);
}
