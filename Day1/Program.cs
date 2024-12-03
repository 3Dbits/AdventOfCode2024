// Part 1
var lines = await File.ReadAllLinesAsync("input.txt");

var (leftList, rightList) = ParseAndSortLists(lines);
int count = leftList.Zip(rightList, (left, right) => Math.Abs(left - right)).Sum();

Console.WriteLine("Part 1 result: {0}", count);

// Part 2
int count2 = leftList.Select(
    (left, index) => left * rightList.Count(right => left == right)).Sum();

Console.WriteLine("Part 2 result: {0}", count2);


static List<int> ParseAndSortColumn(string[] lines, int columnIndex)
{
    return [.. lines
        .Select(line => int.Parse(line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries)[columnIndex]))
        .OrderBy(x => x)];
}

static (List<int> leftList, List<int> rightList) ParseAndSortLists(string[] lines)
{
    var leftList = ParseAndSortColumn(lines, 0);
    var rightList = ParseAndSortColumn(lines, 1);

    return (leftList, rightList);
}