// Part 1
string filePath = "input.txt";
char[][] grid = (await File.ReadAllLinesAsync(filePath))
    .Select(line => line.ToCharArray())
    .ToArray();

int gridLegth = grid.Length;
int gridWidth = grid[0].Length;
var obstraction = '#';

Dictionary<char, Direction> guardMovement = new()
{
    { '^', Direction.Up },
    { '>', Direction.Right },
    { 'v', Direction.Down },
    { '<', Direction.Left }
};

var (x, y, guard) = FindCharPosition(grid, guardMovement);
var obstractions = FindCharPositions(grid, obstraction);
HashSet<(int x, int y)> uniqueCoordinates = [];


(int x, int y)[] directions =
[
    (-1, 0), (0, 1), (1, 0), (0, -1)
];


var position = (x, y);
var direction = guardMovement[guard];
while (true)
{
    (int x, int y) nextPosition = (position.x + directions[(int)direction].x, position.y + directions[(int)direction].y);

    if (IsCordinatesOutOfBounds(nextPosition.x, nextPosition.y))
    {
        break;
    }
    if (obstractions.Contains(nextPosition))
    {
        direction = (Direction)(((int)direction + 1) % 4);
    }
    else
    {
        position = nextPosition;
        uniqueCoordinates.Add(position);
    }
}

Console.WriteLine(uniqueCoordinates.Count);

// Part 2
int guardLoopCount = default;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[i].Length; j++)
    {
        if (obstractions.Contains((i,j)) || !uniqueCoordinates.Contains((i,j)) || (i == x && j == y))
        {
            continue;
        }

        guardLoopCount += StartLoopSimulator(i, j, 7000) ? 1 : 0;
    }
}

bool StartLoopSimulator(int i, int j, int maxLoops)
{
    var position = (x, y);
    var direction = guardMovement[guard];
    int loopCount = default;
    var testObstractions = new HashSet<(int x, int y)>(obstractions) { (i, j) };

    while (loopCount < maxLoops)
    {
        (int x, int y) nextPosition = (position.x + directions[(int)direction].x, position.y + directions[(int)direction].y);

        if (IsCordinatesOutOfBounds(nextPosition.x, nextPosition.y))
        {
            break;
        }
        if (testObstractions.Contains(nextPosition))
        {
            direction = (Direction)(((int)direction + 1) % 4);
        }
        else
        {
            position = nextPosition;
            loopCount++;
        }
    }

    return loopCount == maxLoops;
}

Console.WriteLine(guardLoopCount);
////
bool IsCordinatesOutOfBounds(int x, int y)
{
    return x < 0 || x >= gridLegth || y < 0 || y >= gridWidth;
}

static (int x, int y, char guard) FindCharPosition(char[][] grid, Dictionary<char, Direction> guardMovement)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (guardMovement.ContainsKey(grid[i][j]))
            {
                return (i, j, grid[i][j]);
            }
        }
    }
    throw new InvalidOperationException("Guard not found in the grid.");
}

static List<(int x, int y)> FindCharPositions(char[][] grid, char target)
{
    List<(int x, int y)> positions = new();

    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == target)
            {
                positions.Add((i, j));
            }
        }
    }
    return positions;
}

enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}