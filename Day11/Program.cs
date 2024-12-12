using System.Linq;

string[] numberStrings = (await File.ReadAllTextAsync("input.txt")).Split(' ');
List<long> stones = numberStrings.Select(long.Parse).ToList();
var _input = File.ReadAllText("input.txt").Split(' ').Select(long.Parse).ToList();

// Part 1
for (int i = 0; i < 25; i++)
{
    stones = PlutoStonesChange(stones);
}

Console.WriteLine(stones.Count);

// Part 2
var result = GetStonesAfterIteration(75).Values.Sum();
Console.WriteLine(result);

List<long> PlutoStonesChange(List<long> stones)
{
    List<long> newStones = [];

    foreach (var stoneNumber in stones)
    {
        int stoneNumberLength = stoneNumber.ToString().Length;

        if (stoneNumber == 0)
        {
            newStones.Add(1);
        }
        else if (stoneNumberLength % 2 == 0)
        {
            (long firstHalf, long secondHalf) = SplitNumber(stoneNumber);
            newStones.Add(firstHalf);
            newStones.Add(secondHalf);
        }
        else
        {
            newStones.Add(stoneNumber * 2024);
        }
    }

    return newStones;
}

static (long firstHalf, long secondHalf) SplitNumber(long number)
{
    string numberStr = number.ToString();
    int halfLength = numberStr.Length / 2;

    string firstHalf = numberStr[..halfLength];
    string secondHalf = numberStr[halfLength..];

    return (long.Parse(firstHalf), long.Parse(secondHalf));
}

//long SolveWithCustomPartitioning(List<long> inputList, int repetitions = 1)
//{
//    var repeatedCounts = inputList
//        .GroupBy(x => x)
//        .Where(g => g.Count() > 1)
//        .ToDictionary(g => g.Key, g => g.Count());

//    var distinctList = inputList.Distinct().ToList();

//    List<long> newStones = PlutoStonesChange(distinctList);

//    if (repetitions == 75)
//    {
//        return newStones.Count;
//    }

//    var countResult = SolveWithCustomPartitioning(newStones, repetitions + 1);

//    return countResult;
//}

Dictionary<long, long> GetStonesAfterIteration(int iteration)
{
    var stones = _input.ToDictionary(x => x, x => _input.LongCount(y => y == x));
    stones.TryAdd(1, 0);

    for (var i = 0; i < iteration; i++)
    {
        var modifications = new Dictionary<long, long> { { 1, 0 } };
        foreach (var stone in stones)
        {
            if (stone.Key == 0)
                AddStoneToModifiedList(1, stone.Value, modifications);
            else if (stone.Key.ToString().Length % 2 == 0)
            {
                var stoneString = stone.Key.ToString();
                var leftStone = int.Parse(stoneString[..(stoneString.Length / 2)]);
                var rightStone = int.Parse(stoneString[(stoneString.Length / 2)..]);

                AddStoneToModifiedList(leftStone, stone.Value, modifications);
                AddStoneToModifiedList(rightStone, stone.Value, modifications);
            }
            else
                AddStoneToModifiedList(stone.Key * 2024, stone.Value, modifications);

            stones.Remove(stone.Key);
        }

        foreach (var modification in modifications)
            stones[modification.Key] = modification.Value;
    }

    return stones.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
}

static void AddStoneToModifiedList(long key, long value, Dictionary<long, long> modifications)
{
    if (!modifications.TryAdd(key, value))
        modifications[key] += value;
}