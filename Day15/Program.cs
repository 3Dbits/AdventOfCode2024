using System.Collections.ObjectModel;

string filePathGrid = "grid.txt";
string filePathInput = "input.txt";

char[][] grid = (await File.ReadAllLinesAsync(filePathGrid))
    .Select(line => line.ToCharArray())
    .ToArray();

Queue<Direction> directions = new();
string[] input = await File.ReadAllLinesAsync(filePathInput);

foreach (var line in input)
{
    foreach (var c in line)
    {
        directions.Enqueue(new Direction(c));
    }
}

int gridLegth = grid.Length;
int gridWidth = grid[0].Length;
var boxes = 'O';
var robot = '@';
var gridLimit = '#';

var robotPosition = FindCharPosition(grid, robot);
var boxPositions = FindCharPositions(grid, boxes);
var gridLimitPositions = FindCharPositions(grid, gridLimit);

bool CanMoveBoxes(Coordinate currentPosition, Coordinate direction, Collection<Coordinate> boxPositions, Collection<Coordinate> gridLimitPositions)
{
    var nextPosition = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y
    );

    if (gridLimitPositions.Contains(nextPosition))
    {
        return false;
    }

    if (boxPositions.Contains(nextPosition))
    {
        return CanMoveBoxes(nextPosition, direction, boxPositions, gridLimitPositions);
    }

    return true;
}

void MoveBoxes(Coordinate currentPosition, Coordinate direction, Collection<Coordinate> boxPositions)
{
    var nextPosition = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y
    );

    if (boxPositions.Contains(nextPosition))
    {
        MoveBoxes(nextPosition, direction, boxPositions);
    }

    boxPositions.Remove(currentPosition);
    boxPositions.Add(nextPosition);
}

foreach (var direction in directions)
{
    var nextPosition = new Coordinate(
        robotPosition.X + direction.DirectionCoordinates.X,
        robotPosition.Y + direction.DirectionCoordinates.Y
    );

    if (gridLimitPositions.Contains(nextPosition))
    {
        continue;
    }

    if (boxPositions.Contains(nextPosition) &&
        CanMoveBoxes(nextPosition, direction.DirectionCoordinates, boxPositions, gridLimitPositions))
    {
        MoveBoxes(nextPosition, direction.DirectionCoordinates, boxPositions);
    }
    else if(boxPositions.Contains(nextPosition))
    {
        continue;
    }

    robotPosition = nextPosition;
}

var partOneResult = boxPositions.Select(pos => 100 * pos.X + pos.Y).Sum();

Console.WriteLine(partOneResult);


Coordinate FindCharPosition(char[][] grid, char c)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == c)
            {
                return new Coordinate(i, j);
            }
        }
    }
    return new Coordinate(-1, -1);
}

Collection<Coordinate> FindCharPositions(char[][] grid, char c)
{
    var positions = new Collection<Coordinate>();
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == c)
            {
                positions.Add(new Coordinate(i, j));
            }
        }
    }
    return [.. positions];
}


struct Direction(char direction)
{
    private const char Up = '^';
    private const char Right = '>';
    private const char Down = 'v';
    private const char Left = '<';

    public Coordinate DirectionCoordinates { get; set; } = direction switch
    {
        Up => new Coordinate(-1, 0),
        Right => new Coordinate(0, 1),
        Down => new Coordinate(1, 0),
        Left => new Coordinate(0, -1),
        _ => new Coordinate(0, 0)
    };
}

struct Coordinate(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
}