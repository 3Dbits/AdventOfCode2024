using Day5;

Dictionary<int, List<int>> inputDictionary = await ReadFirstFile("input.txt");

List<List<int>> inputLists = await ReadSecondFile("input2.txt");

// Part 1
List<List<int>> correctLists = inputLists.Where(list => IsValidList(list, inputDictionary)).ToList();
int count = default;

foreach (var list in correctLists)
{
    count += list[list.Count / 2];
}

Console.WriteLine("Part 1 result: {0}", count);

//Part 2
List<List<int>> forFixingList = inputLists.Except(correctLists).ToList();
List<List<int>> fixedList = [];
foreach (var list in forFixingList)
{
    fixedList.Add([.. list.OrderBy(x => x, new CustomIntComparer(inputDictionary))]);
}

int count2 = default;

foreach (var list in fixedList)
{
    count2 += list[list.Count / 2];
}

Console.WriteLine("Part 2 result: {0}", count2);

static bool IsValidList(List<int> list, Dictionary<int, List<int>> rules)
{
    Dictionary<int, int> indexMap = list
        .Select((num, index) => new { Number = num, Index = index })
        .ToDictionary(x => x.Number, x => x.Index);

    foreach (var rule in rules)
    {
        if (!indexMap.ContainsKey(rule.Key))
            continue;

        int keyIndex = indexMap[rule.Key];

        foreach (int value in rule.Value)
        {
            if (!indexMap.ContainsKey(value))
                continue;

            if (indexMap[value] < keyIndex)
            {
                return false;
            }
        }
    }

    return true;
}

static async Task<Dictionary<int, List<int>>> ReadFirstFile(string filename)
{
    Dictionary<int, List<int>> dictionary = [];

    try
    {
        await foreach (string line in File.ReadLinesAsync(filename))
        {
            string[] parts = line.Split('|');
            int key = int.Parse(parts[0]);
            int value = int.Parse(parts[1]);

            if (!dictionary.TryGetValue(key, out List<int>? item))
            {
                dictionary[key] = [value];
            }
            else if (!item.Contains(value))
            {
                item.Add(value);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading first file: {ex.Message}");
    }

    return dictionary;
}

static async Task<List<List<int>>> ReadSecondFile(string filename)
{
    List<List<int>> lists = [];

    try
    {
        await foreach (var line in File.ReadLinesAsync(filename))
        {
            lists.Add(line.Split(',')
                .Select(int.Parse)
                .ToList());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading second file: {ex.Message}");
    }

    return lists;
}