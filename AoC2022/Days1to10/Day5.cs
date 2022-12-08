using System.Runtime.InteropServices;
using AoC;

namespace AoC2022.Days1to10;

public class Day5 : SolverWithLineParser
{
    private readonly List<string> _lines = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 5;

        var data =
            @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";
       
        automaton.RegisterTestDataAndResult(data, "CMZ", question: 1);
        automaton.RegisterTestDataAndResult(data, "MCD", question: 2);
    }

    private (List<List<char>> stacks, List<Instruction> instructions) ParseInstructions(List<string> lines)
    {
        var stacks = new List<List<char>>();
        var instructions = new List<Instruction>();
        
        foreach (var line in lines)
        {
            if (line.StartsWith("move"))
            {
                var parts = line.Split(' ');
                instructions.Add(new Instruction(
                    Count: int.Parse(parts[1]),
                    From: int.Parse(parts[3])-1,
                    To: int.Parse(parts[5])-1));
            }
            else if (line.Length < 2)
            {
            }
            else
            {
                bool TryGetStack(char[] potentialStack, out char item)
                {
                    if (potentialStack[1] is >= 'A' and <= 'Z')
                    {
                        item = potentialStack[1];
                        return true;
                    }

                    item = '\0';
                    return false;
                }

                var boxes = line.Chunk(4).ToList();
                for (int i = 0; i < boxes.Count && stacks.Count < boxes.Count; i++)
                {
                    stacks.Add(new List<char>());
                }
                for (int i = 0; i < boxes.Count; i++)
                {
                    if (TryGetStack(boxes[i], out var item))
                    {
                        stacks[i].Add(item);
                    }
                }
            }
        }
        
        return (stacks, instructions);
    }

    public override object GetAnswer1()
    {
        (List<List<char>> stacks, List<Instruction> instructions) = ParseInstructions(_lines);
        instructions.ForEach((todo) =>
        {
            var items = stacks[todo.From].GetRange(0, stacks[todo.From].Count >= todo.Count ? todo.Count : stacks[todo.From].Count);
            items.Reverse();
            stacks[todo.To].InsertRange(0, items);
            stacks[todo.From].RemoveRange(0, todo.Count);
        });
        return string.Join("", stacks.Select(x => x.First()));
    }

    public override object GetAnswer2()
    {
        (List<List<char>> stacks, List<Instruction> instructions) = ParseInstructions(_lines);
        instructions.ForEach((todo) =>
        {
            var items = stacks[todo.From].GetRange(0, stacks[todo.From].Count >= todo.Count ? todo.Count : stacks[todo.From].Count);
            // items.Reverse(); => no reverse for this question
            stacks[todo.To].InsertRange(0, items);
            stacks[todo.From].RemoveRange(0, todo.Count);
        });
        return string.Join("", stacks.Select(x => x.First()));
    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        var clean = line.Replace("\r", "");
        
        _lines.Add(clean);
    }
    
    private record Instruction(int Count, int From, int To);
}