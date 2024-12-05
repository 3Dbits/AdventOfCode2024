// Part 1
string filePath = "input.txt";
char[][] grid = (await File.ReadAllLinesAsync(filePath))
    .Select(line => line.ToCharArray())
    .ToArray();

int gridLegth = grid.Length;
int gridWidth = grid[0].Length;

(int x, int y)[] directions =
[
    (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1)
];

char[] letters = ['X', 'M', 'A', 'S'];

int resultCount = default;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[i].Length; j++)
    {
        resultCount += WordsInAllDirections(i, j);
    }
}

Console.WriteLine(resultCount);

// Part 2
int resultCount2 = default;

(int x, int y)[] directions2 =
[
    (-1, -1), (1, -1), (-1, 1), (1, 1)
];

char[] letters2 = ['A', 'M', 'M', 'S', 'S'];

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[i].Length; j++)
    {
        resultCount2 += WordsInXDirections(i, j) ? 1 : 0;
    }
}

Console.WriteLine(resultCount2);

int WordsInAllDirections(int startX, int startY)
{
    if (!grid[startX][startY].Equals(letters[0]))
    {
        return default;
    }

    int count = default;

    foreach (var direction in directions)
    {
        int x = startX;
        int y = startY;
        int letterIndex = default;

        while (!IsCordinatesOutOfBounds(x, y))
        {
            x += direction.x;
            y += direction.y;
            letterIndex++;
            if (letterIndex == letters.Length)
            {
                count++;
                break;
            }
            if (IsCordinatesOutOfBounds(x, y) || !grid[x][y].Equals(letters[letterIndex]))
            {
                break;
            }
        }
    }

    return count;
}

bool WordsInXDirections(int startX, int startY)
{
    if (!grid[startX][startY].Equals(letters2[0]))
    {
        return false;
    }

    var letterList = new List<char>();

    foreach (var direction in directions2)
    {
        int x = startX + direction.x;
        int y = startY + direction.y;

        if (IsCordinatesOutOfBounds(x, y))
        {
            return false;
        }

        letterList.Add(grid[x][y]);
    }

    if (letterList.Count == 4)
    {
        string string1 = $"{letterList[0]}{letters2[0]}{letterList[3]}";
        string string2 = $"{letterList[1]}{letters2[0]}{letterList[2]}";

        if ((string1.Equals("MAS") || string1.Equals("SAM"))
            && (string2.Equals("MAS") || string2.Equals("SAM")))
        {
            return true;
        }
    }

    return false;
}

bool IsCordinatesOutOfBounds(int x, int y)
{
    return x < 0 || x >= gridLegth || y < 0 || y >= gridWidth;
}