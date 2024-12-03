using System.Text.RegularExpressions;

// Part 1
string filePath = "input.txt";

List<string> rows = [.. (await File.ReadAllLinesAsync(filePath))];

var regex = new Regex(@"^\d{1,3},\d{1,3}\)");
var results = new List<int>();

foreach (var row in rows)
{
    var parts = row.Split("mul(");
    foreach (var part in parts)
    {
        if (regex.IsMatch(part))
        {
            var numbers = part.Split(',', ')');
            int first = int.Parse(numbers[0]);
            int second = int.Parse(numbers[1]);
            results.Add(first * second);
        }
    }
}

Console.WriteLine("Part 1 result: {0}", results.Sum());

// Part 2

var results2 = new List<int>();
var doFlag = true;

foreach (var row in rows)
{
    var parts = row.Split("mul(");
    foreach (var part in parts)
    {
        if (regex.IsMatch(part) && doFlag)
        {
            var numbers = part.Split(',', ')');
            int first = int.Parse(numbers[0]);
            int second = int.Parse(numbers[1]);
            results2.Add(first * second);
        }
        if(part.Contains("don't()"))
        {
            doFlag = false;
        }
        if (part.Contains("do()"))
        {
            doFlag = true;
        }
    }
}

Console.WriteLine("Part 2 result: {0}", results2.Sum());