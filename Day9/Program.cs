string filePath = "input.txt";
string content = await File.ReadAllTextAsync(filePath);

int[] numbers = content
    .Select(c => int.Parse(c.ToString()))
    .ToArray();

Stack<string> diskSpace = new();
var emptySpace = ".";

for (int i = 0; i < numbers.Length; i++)
{
    if (i.IsOdd())
    {
        for (int j = 0; j < numbers[i]; j++)
        {
            diskSpace.Push(".");
        }
    }
    else
    {
        int numberToRepeat = i == 0 ? 0 : i / 2;
        int repeatCount = numbers[i];
        for (int j = 0; j < repeatCount; j++)
        {
            diskSpace.Push(numberToRepeat.ToString());
        }
    }
}

string[] diskSpaceArray = diskSpace.Reverse().ToArray();

for (int i = diskSpaceArray.Length - 1; i >= 0; i--)
{
    var anyEmptySpace = diskSpaceArray.Take(i).Any(c => c == emptySpace);

    if (diskSpaceArray[i] == emptySpace)
    {
        if (anyEmptySpace)
        {
            continue;
        }
        else
        {
            break;
        }
    }

    if (int.TryParse(diskSpaceArray[i], out int numberInArray))
    {
        if (!anyEmptySpace)
        {
            break;
        }

        int dotIndex = Array.IndexOf(diskSpaceArray, emptySpace);
        if (dotIndex != -1)
        {
            (diskSpaceArray[dotIndex], diskSpaceArray[i]) = (numberInArray.ToString(), diskSpaceArray[dotIndex]);
        }
    }
}

long checksum = default;

for (int i = 0; i < diskSpaceArray.Length; i++)
{
    if (diskSpaceArray[i] == emptySpace)
    {
        break;
    }
    checksum += int.Parse(diskSpaceArray[i]) * i;
}

Console.WriteLine(checksum);

// Part 2
string[] diskSpaceArray2 = diskSpace.Reverse().ToArray();
//Console.WriteLine(string.Join("", diskSpaceArray2));
var switchedPlaces = new List<int>();

for (int i = diskSpaceArray2.Length - 1; i >= 0; i--)
{
    var anyEmptySpace = diskSpaceArray2.Take(i).Any(c => c == emptySpace);

    if (switchedPlaces.Contains(i))
    {
        continue;
    }

    if (diskSpaceArray2[i] == emptySpace)
    {
        if (anyEmptySpace)
        {
            continue;
        }
        else
        {
            break;
        }
    }

    if (int.TryParse(diskSpaceArray2[i], out int numberInArray))
    {
        if (!anyEmptySpace)
        {
            break;
        }

        int lengthOfNumbers = 1;
        while (i - 1 >= 0 && diskSpaceArray2[i - 1] == diskSpaceArray2[i])
        {
            lengthOfNumbers++;
            i--;
        }

        for (int j = 0; j < i; j++)
        {
            if (diskSpaceArray2[j] == emptySpace)
            {
                int spaceLength = 1;
                while (j + 1 < i && diskSpaceArray2[j + 1] == emptySpace)
                {
                    spaceLength++;
                    j++;
                }

                if (spaceLength >= lengthOfNumbers)
                {
                    for (int k = j - spaceLength + 1; k <= j - spaceLength + lengthOfNumbers; k++)
                    {
                        diskSpaceArray2[k] = numberInArray.ToString();
                        switchedPlaces.Add(k);
                    }

                    for (int k = i; k <= i + lengthOfNumbers - 1; k++)
                    {
                        diskSpaceArray2[k] = emptySpace;
                    }

                    break;
                }
            }
        }
    }
}

long checksum2 = default;

for (int i = 0; i < diskSpaceArray2.Length; i++)
{
    if (diskSpaceArray2[i] == emptySpace)
    {
        continue;
    }
    checksum2 += int.Parse(diskSpaceArray2[i]) * i;
}

//Console.WriteLine(string.Join("", diskSpaceArray2));
Console.WriteLine(checksum2);



public static class IntExtensions
{
    public static bool IsOdd(this int number) => number % 2 != 0;

}