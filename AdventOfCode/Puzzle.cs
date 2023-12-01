namespace AdventOfCode;

internal abstract class Puzzle
{
    public string[] Input { get; set; }

    public Puzzle()
    {
        Input = File.ReadAllLines($@"C:\Users\dan.torberg\source\repos\adventofcode2023\AdventOfCode\inputs\input{GetType().Name.Replace("Day", "")}.txt");
    }

    public abstract string Part1();
    public abstract string Part2();
}
