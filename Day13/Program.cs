using System.Numerics;
using System.Text.RegularExpressions;

var filePath = "input.txt";

var machines = ParseMachineData(filePath);

//foreach (var machine in machines)
//{
//    Console.WriteLine($"Button A: {machine.ButtonA.X}, {machine.ButtonA.Y}");
//    Console.WriteLine($"Button B: {machine.ButtonB.X}, {machine.ButtonB.Y}");
//    Console.WriteLine($"Prize: {machine.Prize.X}, {machine.Prize.Y}");
//    Console.WriteLine();
//}

long result = default;
var buttonACost = 3;
var buttonBCost = 1;
var maxTokenPerButton = 100;

foreach (var machine in machines)
{
    var solutions = FindPossiblePaths(
        (machine.ButtonA.X, machine.ButtonA.Y),
        (machine.ButtonB.X, machine.ButtonB.Y),
        (machine.Prize.X, machine.Prize.Y)
    );

    if ( solutions.Count == 0)
    {
        continue;
    }

    int bestSolution = int.MaxValue;

    foreach (var solution in solutions)
    {
        if(solution.Vector1Count > maxTokenPerButton || solution.Vector2Count > maxTokenPerButton)
        {
            continue;
        }
        var total = solution.Vector1Count * buttonACost + solution.Vector2Count * buttonBCost;
        if (total < bestSolution)
        {
            bestSolution = total;
        }
    }

    result += bestSolution == int.MaxValue ? 0 : bestSolution;
}

Console.WriteLine(result);

// Part 2
long part2Result = default;
var part2Machines = new List<Machine>();

foreach (var machine in machines)
{
    part2Machines.Add(new Machine(
        machine.ButtonA,
        machine.ButtonB,
        new Coordinate(
        machine.Prize.X + 10000000000000L,
        machine.Prize.Y + 10000000000000L)
    ));
}

foreach (var machine in part2Machines)
{
    part2Result += Test(
        machine.ButtonA.X, machine.ButtonA.Y,
        machine.ButtonB.X, machine.ButtonB.Y,
        machine.Prize.X, machine.Prize.Y
    );
}

Console.WriteLine(part2Result);

// https://github.com/messcheg/advent-of-code/blob/main/AdventOfCode2024/Day13/Program.cs
long Test(long x1, long y1, long x2, long y2, long x, long y)
{
    long answer2 = 0;

    var B = (y * x1 - x * y1) / (y2 * x1 - x2 * y1);
    var A = (x - B * x2) / x1;
    if (A >= 0 && B >= 0 && B * x2 + A * x1 == x && B * y2 + A * y1 == y) answer2 += A * 3 + B;

    return answer2;
}

static List<(long a, long b)> FindVectorCombinations(long x1, long y1, long x2, long y2, long tx, long ty)
{
    var solutions = new List<(long a, long b)>();
    long denominator = y2 * x1 - y1 * x2;

    // Check for linearly dependent vectors
    if (denominator == 0)
    {
        if (x1 * ty == y1 * tx)
        {
            Console.WriteLine("Infinite solutions exist along the line defined by the vectors.");
        }
        else
        {
            Console.WriteLine("No solution exists due to linearly dependent vectors.");
        }
        return solutions;
    }

    // Calculate numerator for b
    long numeratorB = tx * y1 - ty * x1;

    // Check if b is an integer
    if (numeratorB % denominator != 0)
    {
        return solutions;
    }

    long b = numeratorB / denominator;

    // Ensure b is non-negative
    if (b < 0)
    {
        return solutions;
    }

    // Calculate a using the value of b
    long numeratorA = tx - b * x2;

    // Check if a is an integer
    if (numeratorA % x1 != 0)
    {
        return solutions;
    }

    long a = numeratorA / x1;

    // Ensure a is non-negative
    if (a < 0)
    {
        return solutions;
    }

    solutions.Add((a, b));
    return solutions;
}


// Calculates the Greatest Common Divisor (GCD) using Euclidean algorithm
static BigInteger GCD(BigInteger a, BigInteger b)
{
    a = BigInteger.Abs(a);
    b = BigInteger.Abs(b);
    while (b != 0)
    {
        BigInteger temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

// Extended Euclidean Algorithm to solve linear Diophantine equations
(BigInteger gcd, BigInteger x, BigInteger y) ExtendedGCD(BigInteger a, BigInteger b)
{
    if (a == 0)
    {
        return (b, 0, 1);
    }

    var (gcd, x1, y1) = ExtendedGCD(b % a, a);
    BigInteger x = y1 - (b / a) * x1;
    BigInteger y = x1;

    return (gcd, x, y);
}

// Solve vector path finding with large number optimization
List<(BigInteger v1Count, BigInteger v2Count)> FindVectorPaths(
        (BigInteger x, BigInteger y) vector1,
        (BigInteger x, BigInteger y) vector2,
        (BigInteger x, BigInteger y) target)
{
    // Check if solution is possible using GCD
    BigInteger gcdX = GCD(vector1.x, vector2.x);
    BigInteger gcdY = GCD(vector1.y, vector2.y);

    // Check if target can be reached
    if (target.x % gcdX != 0 || target.y % gcdY != 0)
    {
        return [];
    }

    // Solve the Diophantine equation for x-coordinate
    var (xGcd, x0, y0) = ExtendedGCD(vector1.x, vector2.x);

    // Scale the solution for the target x
    x0 *= target.x / xGcd;
    y0 *= target.x / xGcd;

    var validSolutions = new List<(BigInteger v1Count, BigInteger v2Count)>();

    // Try a few variations of the base solution
    for (int k = -5; k <= 5; k++)
    {
        BigInteger v1Count = x0 + k * (vector2.x / xGcd);
        BigInteger v2Count = y0 - k * (vector1.x / xGcd);

        // Verify both x and y coordinates
        BigInteger currentX = v1Count * vector1.x + v2Count * vector2.x;
        BigInteger currentY = v1Count * vector1.y + v2Count * vector2.y;

        if (currentX == target.x && currentY == target.y)
        {
            validSolutions.Add((v1Count, v2Count));
        }
    }

    return validSolutions;
}



static List<Solution> FindPossiblePaths(
        (long x, long y) vector1,
        (long x, long y) vector2,
        (long x, long y) target,
        int maxIterations = 100)
{
    var solutions = new List<Solution>();

    for (int a = 0; a <= maxIterations; a++)
    {
        for (int b = 0; b <= maxIterations; b++)
        {
            long currentX = a * vector1.x + b * vector2.x;
            long currentY = a * vector1.y + b * vector2.y;

            if (currentX == target.x && currentY == target.y)
            {
                solutions.Add(new Solution
                {
                    Vector1Count = a,
                    Vector2Count = b,
                    RemainingX = 0,
                    RemainingY = 0
                });
            }

            else if (currentX > target.x || currentY > target.y)
            {
                break;
            }
        }
    }

    return solutions;
}

static List<Machine> ParseMachineData(string filePath)
{
    var machines = new List<Machine>();
    var lines = File.ReadAllLines(filePath);

    // Regex patterns to match coordinate formats
    var buttonPattern = new Regex(@"(\w+)\s*:\s*X\+?(\d+),\s*Y\+?(\d+)", RegexOptions.IgnoreCase);
    var prizePattern = new Regex(@"Prize\s*:\s*X\s*=\s*(\d+),\s*Y\s*=\s*(\d+)", RegexOptions.IgnoreCase);

    for (int i = 0; i < lines.Length; i += 4)
    {
        if (i + 2 >= lines.Length) break;

        var buttonAMatch = buttonPattern.Match(lines[i]);
        var buttonBMatch = buttonPattern.Match(lines[i + 1]);
        var prizeMatch = prizePattern.Match(lines[i + 2]);

        if (buttonAMatch.Success && buttonBMatch.Success && prizeMatch.Success)
        {
            var buttonA = new Coordinate(
                int.Parse(buttonAMatch.Groups[2].Value),
                int.Parse(buttonAMatch.Groups[3].Value)
            );

            var buttonB = new Coordinate(
                int.Parse(buttonBMatch.Groups[2].Value),
                int.Parse(buttonBMatch.Groups[3].Value)
            );

            var prize = new Coordinate(
                int.Parse(prizeMatch.Groups[1].Value),
                int.Parse(prizeMatch.Groups[2].Value)
            );

            machines.Add(new Machine(buttonA, buttonB, prize));
        }
    }

    return machines;
}

struct Coordinate(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
}

struct Machine(Coordinate buttonA, Coordinate buttonB, Coordinate prize)
{
    public Coordinate ButtonA { get; set; } = buttonA;
    public Coordinate ButtonB { get; set; } = buttonB;
    public Coordinate Prize { get; set; } = prize;
}

struct Solution
{
    public int Vector1Count { get; set; }
    public int Vector2Count { get; set; }
    public int RemainingX { get; set; }
    public int RemainingY { get; set; }
}