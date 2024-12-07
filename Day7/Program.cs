using System.Collections.Generic;

string filePath = "input.txt";

string[] lines = await File.ReadAllLinesAsync(filePath);

long validSum = default;
long validSum2 = default;

foreach (string line in lines)
{
    string[] parts = line.Split(':');

    if (parts.Length == 2)
    {
        long firstNumber = long.Parse(parts[0].Trim());

        List<long> currentNumberList = parts[1]
            .Trim()
            .Split([' '], StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        // Part 1
        var isValid = IsValidCalculation(firstNumber, currentNumberList);
        if (isValid)
        {
            validSum += firstNumber;
        } else if (IsValidCalculation2(firstNumber, currentNumberList))
        {
            validSum2 += firstNumber;
        }
    }
}

Console.WriteLine(validSum);
Console.WriteLine(validSum2 + validSum);

bool IsValidCalculation(long result, List<long> numbers)
{
    if (numbers.Aggregate((a, b) => a * b) == result)
    {
        return true;
    }
    if (numbers.Sum() == result)
    {
        return true;
    }
    if (IsValidWithTree(ref numbers, numbers.Count - 1, result, numbers[0]))
    {
        return true;
    }

    return false;
}

bool IsValidWithTree(ref List<long> numbers, long lastIndex, long result, long sum, int index = 0)
{
    index++;
    if (index > lastIndex)
    {
        return sum == result;
    }

    var plus = sum + numbers[index];
    var multiply = sum * numbers[index];
    var isPlusValid = false;
    var isMultiplyValid = false;

    if (plus <= result)
    {
        isPlusValid = IsValidWithTree(ref numbers, lastIndex, result, plus, index);
    }
    if (multiply <= result)
    {
        isMultiplyValid = IsValidWithTree(ref numbers, lastIndex, result, multiply, index);
    }

    return isPlusValid || isMultiplyValid;
}

bool IsValidCalculation2(long result, List<long> numbers)
{
    if (numbers.Aggregate((a, b) => a * b) == result
        || numbers.Sum() == result
        || long.Parse(string.Join("", numbers)) == result)
    {
        return true;
    }
    if (IsValidWithTree2(ref numbers, numbers.Count - 1, result, numbers[0]))
    {
        return true;
    }

    return false;
}

bool IsValidWithTree2(ref List<long> numbers, long lastIndex, long result, long sum, int index = 0)
{
    index++;
    if (index > lastIndex)
    {
        return sum == result;
    }

    var plus = sum + numbers[index];
    var multiply = sum * numbers[index];
    var concation = long.Parse($"{sum}{numbers[index]}");
    var isPlusValid = false;
    var isMultiplyValid = false;
    var isConcationValid = false;

    if (plus <= result)
    {
        isPlusValid = IsValidWithTree2(ref numbers, lastIndex, result, plus, index);
    }
    if (multiply <= result)
    {
        isMultiplyValid = IsValidWithTree2(ref numbers, lastIndex, result, multiply, index);
    }
    if (concation <= result)
    {
        isConcationValid = IsValidWithTree2(ref numbers, lastIndex, result, concation, index);
    }

    return isPlusValid || isMultiplyValid || isConcationValid;
}