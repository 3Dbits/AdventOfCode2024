using Day16;
using System.Collections.ObjectModel;

string filePathGrid = "input.txt";

char[][] grid = (await File.ReadAllLinesAsync(filePathGrid))
    .Select(line => line.ToCharArray())
    .ToArray();

const char gridLimit = '#';
const char start = 'S';
const char end = 'E';

var gridLimitPosition = FindCharPosition(grid, gridLimit);
var startPosition = FindCharPosition(grid, start);
var endPosition = FindCharPosition(grid, end);

var pathsToEnd = new Collection<PathToEnd>();

var directions = new List<Coordinate>
{
    new(0, 1),    // Right
    new(0, -1),   // Left
    new(1, 0),    // Down
    new(-1, 0)    // Up
};

var queue = new PriorityQueue<PathState, long>();
var visited = new Dictionary<Coordinate, long>();

queue.Enqueue(new PathState(startPosition, 0, 0, directions[0]), 0);
visited[startPosition] = 0;

while (queue.Count > 0)
{
    var currentState = queue.Dequeue();
    var currentPosition = currentState.Position;

    // If we've reached the end, output the total cost
    if (currentPosition.Equals(endPosition))
    {
        Console.WriteLine(currentState.TotalCost);
        break;
    }

    foreach (var direction in directions)
    {
        var nextPosition = new Coordinate(currentPosition.X + direction.X, currentPosition.Y + direction.Y);

        if (IsValidMove2(nextPosition))
        {
            // Calculate the new number of turns and positions
            long newTurns = currentState.Turns;
            if (currentState.PreviousDirection != null && !direction.Equals(currentState.PreviousDirection))
            {
                newTurns++;
            }
            long newPositions = currentState.Positions + 1;
            long newCost = newPositions + newTurns * 1000;

            // If this path to the next position is better, enqueue it
            if (!visited.TryGetValue(nextPosition, out long value) || value > newCost)
            {
                value = newCost;
                visited[nextPosition] = value;
                queue.Enqueue(new PathState(nextPosition, newPositions, newTurns, direction), newCost);
            }
        }
    }
}

//FindPaths(startPosition, [], 0, directions[1]);
//FindShortestPath();

Console.WriteLine(pathsToEnd.OrderBy(p => p.TotalScore).First().TotalScore);

void FindPaths(Coordinate currentPosition, List<Coordinate> currentPath, long turns = 0, Coordinate? previousDirection = null)
{
    if (currentPosition.Equals(endPosition))
    {
        pathsToEnd.Add(new PathToEnd(new List<Coordinate>(currentPath), turns));
        return;
    }

    foreach (var direction in directions)
    {
        var nextPosition = new Coordinate(currentPosition.X + direction.X, currentPosition.Y + direction.Y);

        if (IsValidMove(nextPosition, currentPath))
        {
            currentPath.Add(nextPosition);
            long newTurns = turns;
            if (previousDirection != null && !direction.Equals(previousDirection))
            {
                newTurns++;
            }
            FindPaths(nextPosition, currentPath, newTurns, direction);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }
}

void FindShortestPath()
{
    var queue = new Queue<(Coordinate Position, List<Coordinate> Path, long Turns, Coordinate? PreviousDirection)>();
    queue.Enqueue((startPosition, [startPosition], 0, directions[1]));
    //visited.Add(startPosition);

    while (queue.Count > 0)
    {
        var (currentPosition, currentPath, turns, previousDirection) = queue.Dequeue();

        if (currentPosition.Equals(endPosition))
        {
            pathsToEnd.Add(new PathToEnd(new List<Coordinate>(currentPath), turns));
            continue;
        }

        foreach (var direction in directions)
        {
            var nextPosition = new Coordinate(currentPosition.X + direction.X, currentPosition.Y + direction.Y);

            if (IsValidMove(nextPosition, currentPath))
            {
                var newPath = new List<Coordinate>(currentPath) { nextPosition };
                long newTurns = turns;
                if (previousDirection != null && !direction.Equals(previousDirection))
                {
                    newTurns++;
                }
                queue.Enqueue((nextPosition, newPath, newTurns, direction));
                //visited.Add(nextPosition);
            }
        }
    }
}

bool IsValidMove(Coordinate position, List<Coordinate> currentPath)
{
    if (position.X < 0 || position.X >= grid.Length || position.Y < 0 || position.Y >= grid[0].Length)
        return false;

    if (grid[position.X][position.Y] == gridLimit)
        return false;

    //if (visited.Contains(position))
       // return false;

    if (position.Equals(startPosition))
        return false;

    return true;
}

bool IsValidMove2(Coordinate position)
{
    if (position.X < 0 || position.X >= grid.Length || position.Y < 0 || position.Y >= grid[0].Length)
        return false;

    if (grid[position.X][position.Y] == gridLimit)
        return false;

    return true;
}

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

Collection<Coordinate> FindCharPositions(char[][] grid, params char[] chars)
{
    var positions = new Collection<Coordinate>();
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (chars.Contains(grid[i][j]))
            {
                positions.Add(new Coordinate(i, j));
            }
        }
    }
    return positions;
}

class PathToEnd(ICollection<Coordinate> path, long numberOfTurns)
{
    private const int TurnScoreMultiplier = 1000;
    public ICollection<Coordinate> Path { get; set; } = path;
    public long NumerOfTurns { get; set; } = numberOfTurns;

    public long TotalScore => Path.Count + NumerOfTurns * TurnScoreMultiplier;
}

public class PathState(Coordinate position, long positions, long turns, Coordinate? previousDirection)
{
    public Coordinate Position { get; } = position;
    public long Positions { get; } = positions;
    public long Turns { get; } = turns;
    public Coordinate? PreviousDirection { get; } = previousDirection;
    public long TotalCost => Positions + Turns * 1000;
}