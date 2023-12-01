using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day1 : Puzzle
{
    public override string Part1()
    {
        var parsedInput = Input.Select(input =>
        {
            var first = input.First(c => int.TryParse(c.ToString(), out var res));
            var last = input.Last(c => int.TryParse(c.ToString(), out var res));

            return int.Parse($"{first}{last}");
        });
        return parsedInput.Sum().ToString();
    }
    public override string Part2()
    {
        var mapping = new Dictionary<string, int>
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
        };
        var parsedInput = Input.Select(input =>
        {
            var pattern = $"({string.Join("|", mapping.Keys)}|[0-9])";
            var first = Regex.Match(input, @pattern);
            var last = Regex.Match(input, @pattern, RegexOptions.RightToLeft);

            var digit = -1;
            var firstDigit = mapping.TryGetValue(first.Value, out digit) ? digit : int.Parse(first.Value);
            var lastDigit = mapping.TryGetValue(last.Value, out digit) ? digit : int.Parse(last.Value);

            return int.Parse($"{firstDigit}{lastDigit}");
        });
        return parsedInput.Sum().ToString();
    }
}
