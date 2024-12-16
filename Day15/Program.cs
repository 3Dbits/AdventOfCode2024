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

const char boxes = 'O';
const char robot = '@';
const char gridLimit = '#';
const char newBoxRight = '[';
const char newBoxLeft = ']';
const char emptySpace = '.';

var robotPosition = FindCharPosition(grid, robot);
var boxPositions = FindCharPositions(grid, boxes);
var gridLimitPositions = FindCharPositions(grid, gridLimit);


// Part 1
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
    else if (boxPositions.Contains(nextPosition))
    {
        continue;
    }

    robotPosition = nextPosition;
}

var partOneResult = boxPositions.Select(pos => 100 * pos.X + pos.Y).Sum();

Console.WriteLine(partOneResult);

// Part 2
var newGrid = TransformGrid(grid);

var robotPosition2 = FindCharPosition(newGrid, robot);
var gridLimitPositions2 = FindCharPositions(newGrid, gridLimit);
var boxPositions2 = FindCharPositions(newGrid, newBoxRight, newBoxLeft);
var boxPairs = FindBoxCooridinates(grid);

foreach (var direction in directions)
{
    var nextPosition = new Coordinate(
        robotPosition2.X + direction.DirectionCoordinates.X,
        robotPosition2.Y + direction.DirectionCoordinates.Y
    );

    if (gridLimitPositions2.Contains(nextPosition))
    {
        continue;
    }

    if (boxPairs.ContainsKey(nextPosition) &&
        (direction.DirectionChar == '>' ||
        direction.DirectionChar == '<') &&
        CanMoveBoxes(nextPosition, direction.DirectionCoordinates, boxPositions2, gridLimitPositions2))
    {
        MoveBoxes2(nextPosition, direction.DirectionCoordinates, boxPositions2);
    }
    else if (boxPairs.ContainsKey(nextPosition) &&
        (direction.DirectionChar == 'v' ||
        direction.DirectionChar == '^') &&
        CanMoveBoxesVertical(nextPosition, direction.DirectionCoordinates, gridLimitPositions2))
    {
        MoveBoxesVertical(nextPosition, direction.DirectionCoordinates);
    }
    else if (boxPairs.ContainsKey(nextPosition))
    {
        continue;
    }

    robotPosition2 = nextPosition;
    //
    //Console.WriteLine(direction.DirectionChar);
    //var newGridTest = CreateNewGrid(robotPosition2, gridLimitPositions2, boxPositions2, newGrid.Length, newGrid[0].Length);
    //PrintGrid(newGridTest);
    //foreach (var box in boxPairs)
    //{
    //    Console.WriteLine($"{box.Key.X}:{box.Key.Y}  => {box.Value.X}:{box.Value.Y}");
    //}
    //
}

long partTwoResult = default;
foreach (var box in boxPairs)
{
    if (box.Value.Y > box.Key.Y)
    {
        boxPairs.Remove(box.Value);
    }
    else
    {
        boxPairs.Remove(box.Key);
    }
}

foreach (var box in boxPairs)
{
    partTwoResult += 100 * box.Key.X + box.Key.Y;
}

Console.WriteLine(partTwoResult);

////////
char[][] CreateNewGrid(
   Coordinate robotPosition,
   Collection<Coordinate> gridLimitPositions,
   Collection<Coordinate> boxPositions,
   int rows,
   int cols)
{
    // Create a new grid filled with '.'
    char[][] newGrid = new char[rows][];
    for (int i = 0; i < rows; i++)
    {
        newGrid[i] = new char[cols];
        Array.Fill(newGrid[i], '.');
    }

    // Place robot
    newGrid[robotPosition.X][robotPosition.Y] = '@';

    // Place grid limits
    foreach (var limitPos in gridLimitPositions)
    {
        newGrid[limitPos.X][limitPos.Y] = '#';
    }

    // Place boxes
    foreach (var boxPos in boxPositions)
    {
        newGrid[boxPos.X][boxPos.Y] = 'O';
    }

    return newGrid;
}

// Method to print the grid to console
void PrintGrid(char[][] grid)
{
    foreach (var row in grid)
    {
        Console.WriteLine(new string(row));
    }
}
//////////
bool CanMoveBoxes(Coordinate currentPosition, Coordinate direction, Collection<Coordinate> boxPositionss, Collection<Coordinate> gridLimitPositions)
{
    var nextPosition = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y
    );

    if (gridLimitPositions.Contains(nextPosition))
    {
        return false;
    }

    if (boxPositionss.Contains(nextPosition))
    {
        return CanMoveBoxes(nextPosition, direction, boxPositionss, gridLimitPositions);
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

void MoveBoxes2(Coordinate currentPosition, Coordinate direction, Collection<Coordinate> boxPositions, int count = 0)
{
    var nextPosition = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y
    );
    count++;

    if (boxPairs.ContainsKey(nextPosition))
    {
        MoveBoxes2(new Coordinate(currentPosition.X, currentPosition.Y + direction.Y), direction, boxPositions, count);
    }

    boxPositions2.Remove(currentPosition);
    boxPositions2.Add(nextPosition);

    boxPairs.Remove(currentPosition);
    boxPairs.Add(nextPosition, count % 2 == 0 ? new Coordinate(currentPosition.X, currentPosition.Y) : new Coordinate(nextPosition.X, nextPosition.Y + direction.Y));
}

void MoveBoxesVertical(Coordinate currentPosition, Coordinate direction)
{
    var positionRight = new Coordinate(
        currentPosition.X,
        currentPosition.Y + 1
    );

    var positionLeft = new Coordinate(
        currentPosition.X,
        currentPosition.Y - 1
     );

    var nextPositionOne = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y
    );

    var nextPositionRight = new Coordinate(
        positionRight.X + direction.X,
        positionRight.Y + direction.Y);

    var nextPositionLeft = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y - 1
    );

    if (boxPairs.TryGetValue(currentPosition, out var valueCurrent) && valueCurrent.Equals(positionRight) && boxPairs.ContainsKey(nextPositionRight))
    {
        MoveBoxesVertical(nextPositionRight, direction);
    }
    if (boxPairs.ContainsKey(nextPositionLeft) && valueCurrent.Equals(positionLeft))
    {
        MoveBoxesVertical(nextPositionLeft, direction);
    }
    if (boxPairs.ContainsKey(nextPositionOne) &&
        (boxPairs.TryGetValue(nextPositionLeft, out var valueLeft) && valueLeft.Equals(nextPositionOne) ||
        boxPairs.TryGetValue(nextPositionRight, out var valueRight) && valueRight.Equals(nextPositionOne)))
    {
        MoveBoxesVertical(nextPositionOne, direction);
    }

    var isRight = valueCurrent.Equals(positionRight);

    boxPairs.Remove(currentPosition);
    boxPairs.Remove(isRight ? positionRight : positionLeft);
    boxPairs.Add(nextPositionOne, isRight ? nextPositionRight : nextPositionLeft);
    boxPairs.Add(isRight ? nextPositionRight : nextPositionLeft, nextPositionOne);

    boxPositions2.Remove(currentPosition);
    boxPositions2.Remove(isRight ? positionRight : positionLeft);
    boxPositions2.Add(nextPositionOne);
    boxPositions2.Add(isRight ? nextPositionRight : nextPositionLeft);
}

bool CanMoveBoxesVertical(Coordinate currentPosition, Coordinate direction, Collection<Coordinate> gridLimitPositions)
{
    var positionRight = new Coordinate(
        currentPosition.X,
        currentPosition.Y + 1
    );

    var positionLeft = new Coordinate(
        currentPosition.X,
        currentPosition.Y - 1
     );

    var nextPositionOne = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y
    );

    var nextPositionRight = new Coordinate(
        positionRight.X + direction.X,
        positionRight.Y + direction.Y);

    var nextPositionLeft = new Coordinate(
        currentPosition.X + direction.X,
        currentPosition.Y + direction.Y - 1
    );

    var isRight = boxPairs.TryGetValue(currentPosition, out var valueNext) && valueNext.Equals(positionRight);
    var forGrid = isRight ? nextPositionRight : nextPositionLeft;
    if (gridLimitPositions.Contains(nextPositionOne) || gridLimitPositions.Contains(forGrid))
    {
        return false;
    }

    var isLeftOk = true;
    var isRightOk = true;
    var isStrightOk = true;

    if (boxPairs.TryGetValue(currentPosition, out var valueCurrent) && valueCurrent.Equals(positionRight) && boxPairs.ContainsKey(nextPositionRight))
    {
        isRightOk = CanMoveBoxesVertical(nextPositionRight, direction, gridLimitPositions);
    }
    if (boxPairs.ContainsKey(nextPositionLeft) && valueCurrent.Equals(positionLeft))
    {
        isLeftOk = CanMoveBoxesVertical(nextPositionLeft, direction, gridLimitPositions);
    }
    if (boxPairs.ContainsKey(nextPositionOne) &&
        (boxPairs.TryGetValue(nextPositionLeft, out var valueLeft) && valueLeft.Equals(nextPositionOne) ||
        boxPairs.TryGetValue(nextPositionRight, out var valueRight) && valueRight.Equals(nextPositionOne)))
    {
        isStrightOk = CanMoveBoxesVertical(nextPositionOne, direction, gridLimitPositions);
    }

    return isRightOk && isLeftOk && isStrightOk;
}

char[][] TransformGrid(char[][] grid)
{
    int rows = grid.Length;

    char[][] newGrid = new char[rows][];

    for (int i = 0; i < rows; i++)
    {
        newGrid[i] = new char[grid[i].Length * 2];

        for (int j = 0; j < grid[i].Length; j++)
        {
            switch (grid[i][j])
            {
                case gridLimit:
                    newGrid[i][j * 2] = gridLimit;
                    newGrid[i][j * 2 + 1] = gridLimit;
                    break;
                case boxes:
                    newGrid[i][j * 2] = newBoxRight;
                    newGrid[i][j * 2 + 1] = newBoxLeft;
                    break;
                case robot:
                    newGrid[i][j * 2] = robot;
                    newGrid[i][j * 2 + 1] = emptySpace;
                    break;
                default:
                    newGrid[i][j * 2] = emptySpace;
                    newGrid[i][j * 2 + 1] = emptySpace;
                    break;
            }
        }
    }

    return newGrid;
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

Dictionary<Coordinate, Coordinate> FindBoxCooridinates(char[][] grid)
{
    var mapping = new Dictionary<Coordinate, Coordinate>();

    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == boxes)
            {
                var coord1 = new Coordinate(i, j * 2);
                var coord2 = new Coordinate(i, j * 2 + 1);

                mapping[coord1] = coord2;
                mapping[coord2] = coord1;
            }
        }
    }

    return mapping;
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

    public char DirectionChar { get; set; } = direction;
}

struct Coordinate(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;

    public override bool Equals(object obj) =>
        obj is Coordinate coordinate &&
               X == coordinate.X &&
               Y == coordinate.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);
}