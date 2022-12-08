using AoC;

namespace AoC2022.Days1to10;

public class Day6 : SolverWithParser
{
    private List<char> _signal = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 6;

        automaton.RegisterTestDataAndResult("mjqjpqmgbljsphdztnvjfqwrcgsmlb",
            7,
            1);
        automaton.RegisterTestDataAndResult("bvwbjplbgvbhsrlpgdmjqwftvncz",
            5,
            1);
        automaton.RegisterTestDataAndResult("nppdvjthqldpwncqszvftbrmjlhg",
            6,
            1);
        automaton.RegisterTestDataAndResult("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",
            10,
            1);
        automaton.RegisterTestDataAndResult("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw",
            11,
            1);
        
        automaton.RegisterTestDataAndResult("mjqjpqmgbljsphdztnvjfqwrcgsmlb",
            19,
            2);
        automaton.RegisterTestDataAndResult("bvwbjplbgvbhsrlpgdmjqwftvncz",
            23,
            2);
    }

    private bool TryGetSignalStart(List<char> signal, out HashSet<char> marker)
    {
        marker = new HashSet<char>();

        foreach (var t in signal)
        {
            if (!marker.Add(t))
            {
                return false;
            }
        }

        return true;
    }

    public override object GetAnswer1()
    {
        for (int i = 0; i < _signal.Count; i++)
        {
            if (TryGetSignalStart(_signal.GetRange(i, 4), out var marker))
            {
                return i + 4;
            }
        }
        throw new Exception("failed");
    }

    public override object GetAnswer2()
    {
        for (int i = 0; i < _signal.Count; i++)
        {
            if (TryGetSignalStart(_signal.GetRange(i, 14), out var marker))
            {
                return i + 14;
            }
        }
        throw new Exception("failed");
    }

    protected override void Parse(string data)
    {
        _signal = data.Trim().ToCharArray().ToList();
    }
}