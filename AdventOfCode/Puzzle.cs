namespace AdventOfCode;

internal abstract class Puzzle
{
    public string[] Input { get; set; }

    public Puzzle()
    {
        Input = File.ReadAllLines($@"{Directory.GetCurrentDirectory()}\inputs\input{GetType().Name.Replace("Day", "")}.txt");
    }

    public abstract string Part1();
    public abstract string Part2();
}
