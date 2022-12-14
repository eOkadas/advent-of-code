using System.Diagnostics;
using System.Runtime.InteropServices;
using AoC;

namespace AoC2022.Days11to20;

public class Day14 : SolverWithLineParser
{
    private readonly HashSet<Coordinates> _cave = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 14;

        var data = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";
        automaton.RegisterTestDataAndResult(data, 24, 1);
        automaton.RegisterTestDataAndResult(data, 93, 2);
    }

    private void CountSandFlow((Coordinates start, Coordinates end) edges)
    {
        var sandOrigin = new Coordinates(500, 0);
        do
        {
            var sandCursor = sandOrigin with { };

            while (sandCursor.Y != edges.end.Y)
            {
                if (!_cave.Contains(sandCursor with { Y = sandCursor.Y + 1 }))
                {
                    sandCursor = sandCursor with { Y = sandCursor.Y + 1 };
                    continue;
                }

                if (!_cave.Contains(new Coordinates(sandCursor.X - 1, sandCursor.Y + 1)))
                {
                    sandCursor = new Coordinates(sandCursor.X - 1, sandCursor.Y + 1);
                    continue;
                }
                if (!_cave.Contains(new Coordinates(sandCursor.X + 1, sandCursor.Y + 1)))
                {
                    sandCursor = new Coordinates(sandCursor.X + 1, sandCursor.Y + 1);
                    continue;
                }

                break;
            }

            if (sandCursor.Y == edges.end.Y)
            {
                break;
            }

            _cave.Add(sandCursor with { symbol = 'o' });
        } while (!_cave.Contains(new Coordinates(500, 0))); ;
    }

    public override object GetAnswer1()
    {
        (Coordinates start, Coordinates end) edges = new(
            new Coordinates(_cave.Min(s => s.X), 0),
            new Coordinates(_cave.Max(s => s.X), _cave.Max(s => s.Y)));
        CountSandFlow(edges);

        return _cave.Count(_ => _.symbol == 'o');
    }

    //26845
    public override object GetAnswer2()
    {
        void Print()
        {
            var minX = _cave.Min(s => s.X);
            var maxX = _cave.Max(s => s.X);
            var minY = _cave.Min(s => s.Y);
            var maxY = _cave.Max(s => s.Y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var c = new Coordinates(x, y);
                    Debug.Write(_cave.TryGetValue(c, out var value) ? value.symbol : '.');
                }

                Debug.WriteLine(string.Empty);
            }
        }

       CountSandFlow(new(
            new Coordinates(_cave.Min(s => s.X), 0),
            new Coordinates(_cave.Max(s => s.X), _cave.Max(s => s.Y))));

        (Coordinates start, Coordinates end) edges = new(
            new Coordinates(_cave.Min(s => s.X), 0),
            new Coordinates(_cave.Max(s => s.X), _cave.Max(s => s.Y)));

        Print();

        var floorY = _cave.Max(s => s.Y) + 2;
        
        for (var x = edges.start.X - floorY; x < edges.end.X + floorY; x++)
        {
            _cave.Add(new (x, floorY));
        }
        
        CountSandFlow(new(
            new Coordinates(_cave.Min(s => s.X), 0),
            new Coordinates(_cave.Max(s => s.X), _cave.Max(s => s.Y))));
        
        Print();

        return _cave.Count(_ => _.symbol == 'o');
    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        DrawMap(line.Trim());

        void DrawMap(string input)
        {
            var segments = input.Split(" -> ");
            for (var i = 0; i < segments.Length - 1; i++)
            {
                var startInput = segments[i].Split(',');
                var start = new Coordinates(int.Parse(startInput[0]), int.Parse(startInput[1]));

                var endInput = segments[i + 1].Split(',');
                var end = new Coordinates(int.Parse(endInput[0]), int.Parse(endInput[1]));

                var direction = start.X == end.X
                    ? (X: 0, Y: Math.Sign(end.Y - start.Y))
                    : (X: Math.Sign(end.X - start.X), Y: 0);

                _cave.Add(new(start.X, start.Y));

                var cursor = start;
                while (cursor.X != end.X || cursor.Y != end.Y)
                {
                    cursor = new Coordinates(X: cursor.X + direction.X, Y: cursor.Y + direction.Y);

                    _cave.Add(cursor with { });
                }
            }
        }
    }

    [DebuggerDisplay("X = {X}, Y = {Y}")]
    private sealed record Coordinates(int X, int Y, char symbol = '#')
    {
        public bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    };
}