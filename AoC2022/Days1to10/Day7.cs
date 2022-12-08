using System.Collections.Immutable;
using System.Diagnostics;
using AoC;

namespace AoC2022.Days1to10;

public class Day7 : SolverWithLineParser
{
    private readonly List<string> _input = new();

    public override void SetupRun(Automaton automaton)
    {
        automaton.Day = 7;

        var testData = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

        // Parse testData into _input
        // for each line in _input do
        //   if line starts with $ cd then add a new folder to the current folder tree
        //   if line starts with number then add a file to the current folder

        automaton.RegisterTestDataAndResult(testData, 95437, question: 1);
        
        automaton.RegisterTestDataAndResult(testData, 24933642, question: 2);
    }

    private (Folder folder, int totalWeight) BuildTree(List<string> input)
    {
        Folder? currentFolder = null;
        int totalWeight = 0;
        foreach (var cmd in input)
        {
            if (cmd.StartsWith("$ cd .."))
            {
                currentFolder = currentFolder!.Parent;
            }
            else if (cmd.StartsWith("$ cd "))
            {
                if (cmd[5..] == "/")
                {
                    currentFolder = new Folder(cmd[5..]);
                }
                else
                {
                    currentFolder = currentFolder!.Folders.Single(_ => _.Id == cmd[5..]);
                }
            }

            if (cmd.StartsWith("dir "))
            {
                currentFolder!.AddSubFolder(cmd[4..]);
            }

            if (cmd[0] is >= '1' and <= '9')
            {
                var fileSize = int.Parse(cmd.Split(' ')[0]);
                currentFolder!.Weight += fileSize;
                totalWeight += fileSize;
                SpreadWeight(currentFolder, fileSize);
            }
        }

        while (currentFolder!.HasParent)
        {
            currentFolder = currentFolder.Parent;
        }
        
        return (currentFolder, totalWeight);
    }

    private void SpreadWeight(Folder currentFolder, int fileSize)
    {
        var folderCursor = currentFolder;
        while (folderCursor.HasParent)
        {
            folderCursor.Parent!.Weight += fileSize;
            folderCursor = folderCursor.Parent;
        }
    }

    private List<Folder> FlattenFolderTree(Folder root)
    {
        List<Folder> folders = new();

        folders.Add(root);
        foreach (var folder in root.Folders)
        {
            folders.AddRange(FlattenFolderTree(folder));
        }

        return folders;
    }

    public override object GetAnswer1()
    {
        var rootFolder = BuildTree(_input).folder;
        var folders = FlattenFolderTree(rootFolder);

        return folders.Where(_ => _.Weight <= 100000).Select(_ => _.Weight).Sum();
    }

    public override object GetAnswer2()
    {
        const int maxSize = 70_000_000;
        const int minEmpty = 30_000_000;
        
        var tree = BuildTree(_input);
        
        var rootFolder = tree.folder;
        var folders = FlattenFolderTree(rootFolder);

        int freeSpace = maxSize - tree.totalWeight;

        var sortedFolders = folders.Select(_ => (id: _.Id, weight: _.Weight))
            .OrderBy(_ => _.weight);
            
        var result = sortedFolders.First(_ => freeSpace + _.weight >= minEmpty);
        
        return result.weight;
    }

    protected override void ParseLine(string line, int index, int lineCount)
    {
        _input.Add(line.TrimEnd());
    }

    [DebuggerDisplay("{Id} ({Weight})")]
    private class Folder
    {
        public Folder(string id)
        {
            Id = id;
        }

        public string Id { get; set; }

        public int Size { get; set; }
        public List<Folder> Folders { get; } = new();
        public Folder? Parent { get; set; }
        public int Weight { get; set; }

        public bool HasParent => Parent != null;

        public Folder AddSubFolder(string id)
        {
            var folder = new Folder(id);
            folder.Parent = this;
            Folders.Add(folder);
            return folder;
        }
    }
}