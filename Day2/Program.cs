// Part 1
string filePath = "input.txt";

var rows = new List<List<int>>();

await foreach (var line in File.ReadLinesAsync(filePath))
{
    rows.Add(line.Split(' ')
                 .Select(int.Parse)
                 .ToList());
}

int count = default;

foreach (var row in rows)
{
    count += IsValidRow( row) ? 1 : 0;
}

Console.WriteLine("Part 1 result: {0}", count);

// Part 2
int count2 = default;

foreach (var row in rows)
{
    var combinations = GetCombinationsWithoutOne(row);

    if (combinations.Exists(list => IsValidRow(list)))
        count2++;
}

Console.WriteLine("Part 2 result: {0}", count2);

static List<List<int>> GetCombinationsWithoutOne(List<int> originalList)
{
    var combinations = new List<List<int>>();

    for (int i = 0; i < originalList.Count; i++)
    {
        var newList = new List<int>(originalList);
        newList.RemoveAt(i);
        combinations.Add(newList);
    }

    return combinations;
}

static bool IsValidRow(List<int> row)
{
    var isAscendingOrder = row[0] < row[1];

    var safeRow = true;
    for (int i = 1; i < row.Count; i++)
    {
        var difference = Math.Abs(row[i] - row[i - 1]);

        if ((difference <= 0 || difference >= 4) ||
            (!isAscendingOrder && row[i - 1] < row[i]) ||
            (isAscendingOrder && row[i - 1] > row[i]))
        {
            safeRow = false;
            break;
        }
    }

    return safeRow;

}