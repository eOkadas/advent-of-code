using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using AoC;

namespace AoC2022.Days1to10;

public class Day9 : SolverWithLineParser
{
    private List<Instructions> _instructions = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 9;

        var data = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

        automaton.RegisterTestDataAndResult(data, 13, 1);
        automaton.RegisterTestDataAndResult(data, 1, 2);

        automaton.RegisterTestDataAndResult(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20", 36, 2);
    }

    public Coordinates ComputeTailMovement(Coordinates head, Coordinates tail)
    {
        if (head.IsTouching(tail))
            return tail;
        return tail.NewCoordinatesTowards(head);
    }


    public override object GetAnswer1()
    {
        HashSet<Coordinates> visited = new();
        (Coordinates head, Coordinates tail) rope = new(new Coordinates(0, 0), new Coordinates(0, 0));

        visited.Add(rope.tail);

        foreach (var instruction in _instructions)
        {
            rope.head = instruction.Move(rope.head);

            while (!rope.head.IsTouching(rope.tail))
            {
                rope.tail = ComputeTailMovement(rope.head, rope.tail);
                visited.Add(rope.tail);
            }
        }

        return visited.Count;
    }

    public override object GetAnswer2()
    {
        HashSet<Coordinates> visited = new();
        List<Coordinates> rope = Enumerable.Range(0, 10).Select(_ => (new Coordinates(0,0))).ToList();

        visited.Add(rope[0]);

        foreach (var instruction in _instructions)
        {
            rope[0] = instruction.Move(rope[0]);
            for (var cursor = 1; cursor < rope.Count; cursor++)
            {
                while (!rope[cursor - 1].IsTouching(rope[cursor]))
                {
                    for (int i = cursor; i < rope.Count; i++)
                    {
                        rope[i] = ComputeTailMovement(rope[i - 1], rope[i]);
                    }

                    visited.Add(rope.Last());
                }
            }
        }

        return visited.Count;
    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        var parsedInstruction = line.Trim().Split(' ');
        _instructions.Add(new Instructions(parsedInstruction[0][0], int.Parse(parsedInstruction[1])));
    }

    private record Instructions(char Direction, int Distance)
    {
        public Coordinates Move(Coordinates coordinates)
        {
            var newCoordinates = this switch
            {
                { Direction: 'U' } => coordinates with { Y = coordinates.Y - Distance },
                { Direction: 'D' } => coordinates with { Y = coordinates.Y + Distance },
                { Direction: 'L' } => coordinates with { X = coordinates.X - Distance },
                { Direction: 'R' } => coordinates with { X = coordinates.X + Distance },
                _ => throw new ArgumentException()
            };
            return newCoordinates;
        }
    }
}

[DebuggerDisplay("{X}, {Y}")]
public record Coordinates(int X, int Y)
{
    public bool IsTouching(Coordinates other)
    {
        return Math.Abs(other.X - X) <= 1 && Math.Abs(other.Y - Y) <= 1;
    }

    public Coordinates NewCoordinatesTowards(Coordinates other)
    {
        var x = X;
        var y = Y;
        if (X < other.X)
            x++;
        else if (X > other.X)
            x--;
        if (Y < other.Y)
            y++;
        else if (Y > other.Y)
            y--;
        return new Coordinates(x, y);
    }
}