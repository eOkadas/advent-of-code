using AoC;

namespace AoC2022.Days1to10;

public class Day4 : SolverWithLineParser
{
    private readonly List<(Area, Area)> Pairs = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 4;
        var data =
            @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8
";
        automaton.RegisterTestDataAndResult(data, 2, question: 1);
        automaton.RegisterTestDataAndResult(data, 4, question: 2);
    }

    private bool CheckFullOverlap(Area a, Area b)
    {
        return
            a.Min >= b.Min && a.Max <= b.Max;
    }

    private bool CheckAnyOverlap(Area a, Area b)
    {
        return
            a.Min >= b.Min && a.Min <= b.Max;
    }
    
    public override object GetAnswer1()
    {
        return Pairs.Count(pair => CheckFullOverlap(pair.Item1, pair.Item2)
                            || CheckFullOverlap(pair.Item2, pair.Item1));
    }

    public override object GetAnswer2()
    {
        return Pairs.Count(pair => CheckAnyOverlap(pair.Item1, pair.Item2)
                                   || CheckAnyOverlap(pair.Item2, pair.Item1));
    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        var parts = line.Trim().Split(',');
        Pairs.Add((new Area(parts[0].Split('-')), new Area(parts[1].Split('-'))));
    }

    private record Area(int Min, int Max)
    {
        public Area(string[] split) : this( int.Parse(split[0]), int.Parse(split[1]))
        {
            ArgumentOutOfRangeExceptionWithCheck.ThrowIfNotRange(split, 2);
        }
    }
}