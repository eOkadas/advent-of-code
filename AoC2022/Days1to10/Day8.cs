using System.Diagnostics;
using AoC;
using Microsoft.VisualBasic;

namespace AoC2022.Days1to10;

public class Day8 : SolverWithLineParser
{
    private readonly List<List<Tree>> _forest = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 8;

        var data = @"30373
25512
65332
33549
35390";
        automaton.RegisterTestDataAndResult(data, 21, 1);
        automaton.RegisterTestDataAndResult(data, 8, 2);
    }

    public override object GetAnswer1()
    {
        List<Tree> visibleTrees = new List<Tree>();

        AddEdges(_forest, visibleTrees);

        void AddEdges(List<List<Tree>> forest, List<Tree> themVisibleTrees)
        {
            themVisibleTrees.AddRange(forest[0].Select(_ => _));
            themVisibleTrees.AddRange(forest[^1].Select(_ => _));

            themVisibleTrees.AddRange(Enumerable.Range(1, _forest[1].Count - 2)
                .Select(x => _forest[x][0])
                .ToList());
            themVisibleTrees.AddRange(Enumerable.Range(1, _forest[1].Count - 2)
                .Select(x => _forest[x][^1])
                .ToList());
        }


        for (int i = 0, j = 1; i < _forest.Count; i++)
        {
            //if current tree is bigger than previous biggest tree then add it to the list, and save it a the biggest tree
            var biggestTree = _forest[i][0];
            var reverseBiggestTree = _forest[i][^1];
            CheckIfTreeIsBiggerThanPrevious(ref visibleTrees, ref biggestTree, ref reverseBiggestTree, _forest[i], j);
        }

        for (int i = 1, j = 0; j < _forest.Count; j++)
        {
            var biggestTree = _forest[0][j];
            var reverseBiggestTree = _forest[^1][j];
            var treeLine = Enumerable.Range(0, _forest[i].Count)
                .Select(x => _forest[x][j]).ToList();

            CheckIfTreeIsBiggerThanPrevious(ref visibleTrees, ref biggestTree, ref reverseBiggestTree, treeLine, i);
        }

        return visibleTrees.Count;
    }

    private void CheckIfTreeIsBiggerThanPrevious(ref List<Tree> themVisibleTrees,
        ref Tree biggestTree,
        ref Tree reverseBiggestTree,
        List<Tree> treeLine,
        int index)
    {
        if (biggestTree.Size < treeLine[index].Size)
        {
            biggestTree = treeLine[index];
            if (!themVisibleTrees.Contains(biggestTree))
            {
                themVisibleTrees.Add(biggestTree);
            }
        }

        if (index < treeLine.Count - 1)
        {
            CheckIfTreeIsBiggerThanPrevious(ref themVisibleTrees, ref biggestTree, ref reverseBiggestTree, treeLine,
                index + 1);
        }

        if (reverseBiggestTree.Size < treeLine[index].Size)
        {
            reverseBiggestTree = treeLine[index];
            if (!themVisibleTrees.Contains(reverseBiggestTree))
            {
                themVisibleTrees.Add(reverseBiggestTree);
            }
        }
    }

    public override object GetAnswer2()
    {
        var biggestScore = 0;
        for (var i = 1; i < _forest.Count - 1; i++)
        {
            for (var j = 1; j < _forest[i].Count - 1; j++)
            {
                (int up, int down, int left, int right) visibleCount = ( 1, 1, 1, 1);
                
                for (var cursor = i - 1; cursor > 0 && _forest[cursor][j].Size < _forest[i][j].Size; cursor--)
                {
                    visibleCount.up++;
                }

                for (var cursor = i + 1; cursor < _forest.Count-1 && _forest[cursor][j].Size < _forest[i][j].Size; cursor++)
                {
                    visibleCount.down++;
                }

                for (var cursor = j - 1; cursor > 0 && _forest[i][cursor].Size < _forest[i][j].Size; cursor--)
                {
                    visibleCount.left++;
                }

                for (var cursor = j + 1; cursor < _forest[i].Count-1 && _forest[i][cursor].Size < _forest[i][j].Size; cursor++)
                {
                    visibleCount.right++;
                }

                var score = visibleCount.up * visibleCount.down * visibleCount.left * visibleCount.right;
                if (score > biggestScore)
                    biggestScore = score;
            }
        }

        return biggestScore;
    }

    protected override void ParseLine(string line, int indei, int lineCount)
    {
        _forest.Add(line.TrimEnd().Select(_ => new Tree((int)Char.GetNumericValue(_))).ToList());
    }

    [DebuggerDisplay("size={Size}")]
    private class Tree
    {
        public readonly int Size;

        public Tree(int size)
        {
            Size = size;
        }
    }
}