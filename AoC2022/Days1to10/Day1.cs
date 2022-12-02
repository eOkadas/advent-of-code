using AoC;

namespace AoC2022.Days1to10;

public class Day1 : SolverWithLineParser
{
    List<List<int>> baskets = new() { new List<int>() };

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 1;

        var testData = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000
";
        automaton.RegisterTestDataAndResult(testData, expected: 24000, 1);
        
        automaton.RegisterTestDataAndResult(testData, expected: 45000, 2);
    }


    public override object GetAnswer1() => baskets.Select(calories => calories.Sum()).Max();

    public override object GetAnswer2()
    => baskets.Select(calories => calories.Sum()).OrderByDescending(x => x).Take(3).Sum();

    
    protected override void ParseLine(string line, int index, int lineCount)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            baskets[^1].Add(int.Parse(line));
        }
        else
        {
            // if line is empty, add a new list
            baskets.Add(new List<int>());
        }
    }
}