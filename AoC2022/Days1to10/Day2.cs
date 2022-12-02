using System.Formats.Asn1;
using AoC;

namespace AoC2022.Days1to10;

public class Day2 : SolverWithLineParser
{
    private readonly Dictionary<string, int> _scores = new();
    private readonly List<Matchup> _moves = new();
    
    
    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 2;

        var testData = @"A Y
B X
C Z";
        automaton.RegisterTestDataAndResult(testData, 15, question: 1);
        
        automaton.RegisterTestDataAndResult(testData, 12, question: 2);
    }
    
    public override object GetAnswer1()
    {
        //23 chars difference between 'A' and 'X'
        var elves = new [] { 'A', 'B',  'C' };
        var player = new [] { 'X', 'Y', 'Z' };
        
        var winningMoves = new List<string>{"C A", "A B", "B C"};
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var moveScore = 
                    elves[i] == (char)(player[j]-23) ? 3 :
                    winningMoves.Contains($"{elves[i]} {(char)(player[j]-23)}") ? 6 
                    : 0;
                
                _scores.Add($"{elves[i]} {player[j]}", j+1 + moveScore);
            }
        }

        return _moves.Select(_ => _scores[$"{_.Elf} {_.Player}"]).Sum();
    }

    public override object GetAnswer2()
    {
        int getWinningMove(Matchup matchUp)
        {
            int ApplyBoundaries(int move)
            {
                if (move < 'A') move += 3;
                if (move > 'C') move -= 3;
                return move;
            }

            int GetScoreValue(int move) => move - '@';
            
            var score =  matchUp switch
            {
                { Player: 'X' } => GetScoreValue(ApplyBoundaries(matchUp.Elf + 2)),
                { Player: 'Y' } => GetScoreValue(matchUp.Elf) + 3,
                { Player: 'Z' } => GetScoreValue(ApplyBoundaries(matchUp.Elf - 2)) + 6,
                _ => throw new ArgumentException(matchUp.Player.ToString())
            };
            
            return score;
        }
        

        return _moves.Select(getWinningMove).Sum();
    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        _moves.Add(new (line[0], line[2]));
    }
    
    private record Matchup(char Elf, char Player);
}