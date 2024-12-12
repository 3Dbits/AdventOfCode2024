
string filePath = "input.txt";
char[][] grid = (await File.ReadAllLinesAsync(filePath))
    .Select(line => line.ToCharArray())
    .ToArray();

int gridLegth = grid.Length;
int gridWidth = grid[0].Length;

(int x, int y)[] directions =
[
    (1, 0), (0, 1), (-1, 0), (0, -1)
];

Dictionary<(int, int), bool> visitedCoordinates = [];
Dictionary<(int, int), int> regionData = [];
var debugList = new List<(int, int)>();
long resultCount = default;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[i].Length; j++)
    {
        if (visitedCoordinates.ContainsKey((i, j)))
        {
            continue;
        }

        GetRegionFieldsWithNeighbours(i, j);
        var filedsCount = regionData.Count;
        var fenceCount = 4 * filedsCount - regionData.Values.Sum();
        debugList.Add((filedsCount, fenceCount));
        resultCount += filedsCount * fenceCount;
        regionData.Clear();
    }
}

Console.WriteLine(resultCount);

void GetRegionFieldsWithNeighbours(int i, int j)
{

    if (visitedCoordinates.ContainsKey((i, j)))
    {
        return;
    }

    visitedCoordinates.Add((i, j), true);
    regionData.Add((i, j), 0);

    foreach (var direction in directions)
    {
        int x = i + direction.x;
        int y = j + direction.y;
        if (IsCordinatesOutOfBounds(x, y))
        {
            continue;
        }
        if (grid[x][y] == grid[i][j])
        {
            regionData[(i, j)] = regionData[(i, j)] + 1;

            GetRegionFieldsWithNeighbours(x, y);
        }
    }
}

bool IsCordinatesOutOfBounds(int x, int y)
{
    return x < 0 || x >= gridLegth || y < 0 || y >= gridWidth;
}