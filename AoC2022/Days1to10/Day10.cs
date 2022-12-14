using AoC;

namespace AoC2022.Days1to10;

public class Day10 : SolverWithLineParser
{
    private readonly List<Instruction> _instructions = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 10;

        var data = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
        automaton.RegisterTestDataAndResult(data, 13140, 1);
        // automaton.RegisterTestDataAndResult(data, 13140, 2);
    }

    public override object GetAnswer1()
    {
        var cycleCount = 0;
        var x = 1;

        var signals = new List<(int strength, int cycleIndex)>();

        foreach (var instruction in _instructions)
        {
            if (instruction.WorkType.IsNoOp)
            {
                CycleThrough(1);
                continue;
            }
            
            CycleThrough(2);
            x += instruction.Factor!.Value;

            void CycleThrough(int addCycles)
            {
                for(int i = 0; i < addCycles; i++)
                {
                    cycleCount++;
                    if (cycleCount == 20 || (cycleCount - 20) % 40 == 0)
                    {
                        signals.Add((x * cycleCount, cycleCount));
                    }
                }
            }
        }

        return signals.Sum(_ => _.strength);
    }

    public override object GetAnswer2()
    {
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("");

        // Sprite is 3 Pixels wide
        // Screen is 40 wide and 6 high
        // X is the middle of the sprite => it starts as One
        //   because the middle of the sprite is the first pixel location
        //   ###.......
        
        var cycleCount = 0;
        int CaretPosition() => cycleCount % 40 == 0 ? 40 -1 : cycleCount % 40 -1;
        var spritePosition = 1;

        var screen = new List<Char[]>() { new char[40] };
        
        foreach (var instruction in _instructions)
        {
            if (instruction.WorkType.IsNoOp)
            {
                CycleThrough(1, null);
                continue;
            }
            
            CycleThrough(2, instruction.Factor!.Value);
            spritePosition += instruction.Factor!.Value;
            
            
            void CycleThrough(int addCycles, int? factor)
            {
                for(int i = 0; i < addCycles; i++)
                {
                    cycleCount++;
                    //.......###..
                    
                    if(CaretPosition() <= spritePosition+1 && CaretPosition() >= spritePosition-1)
                    {
                        screen.Last()[CaretPosition()] = '#';
                    }
                    else
                    {
                        screen.Last()[CaretPosition()] = '.';
                    }
                    
                    if(cycleCount % 40 == 0)
                    {
                        screen.Add(new char[40]);
                    }
                }
            }
        }

        foreach (var line in screen)
        {
            Console.WriteLine(new string(line));
        }
        
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("");
        return 0;
        
//         
// ####..##....##..##..###....##.###..####.
// #....#..#....#.#..#.#..#....#.#..#.#....
// ###..#.......#.#..#.#..#....#.#..#.###..
// #....#.......#.####.###.....#.###..#....
// #....#..#.#..#.#..#.#....#..#.#.#..#....
// #.....##...##..#..#.#.....##..#..#.####.

    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        _instructions.Add(new Instruction(line.Trim()));
    }

    public record Instruction(WorkType WorkType, int? Factor)
    {
        public Instruction(string line) :
            this(new WorkType(line.Split(' ')[0]),
                line.Split(' ').Length == 2
                    ? int.Parse(line.Split(" ")[1])
                    : null)
        {
            ArgumentOutOfRangeExceptionWithCheck.ThrowIfNotRange(line.Split(), 2);

            var lineArray = line.Split(' ');

            WorkType = new WorkType(lineArray[0]);

            if (lineArray.Count() == 2)
            {
                Factor = int.Parse(lineArray[1]);
            }
        }
    }

    public record WorkType
    {
        public string Value { private get; init; }

        public WorkType(string value)
        {
            ArgumentOutOfRangeExceptionWithCheck.ThrowIfNotValid(value,
                new[] { "addx", "noop" });
            Value = value;
        }

        public const string AddX = "addx";
        public const string Noop = "noop";

        public bool IsAddX => Value == AddX;
        public bool IsNoOp => Value == Noop;
    }
}