
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace AdventOfCode;

internal class Day3 : Puzzle
{
    public record Char(CharType type, string value, int length, int index);
    public enum CharType { Number = 0, Dot = 1, Symbol = 2 };

    public List<List<Char>> ParseInput() {
        return Input.Select(line => {
            var lineChars = new List<Char>();
            for (var i = 0; i < line.Length;) {
                var c = line[i];
                if (int.TryParse(c.ToString(), out var number)) 
                {
                    var fullNumber = line[i++].ToString();
                    while(i < line.Length && int.TryParse(line[i].ToString(), out number))
                    {
                        fullNumber += line[i++].ToString();
                    }

                    lineChars.AddRange(Enumerable.Range(0, fullNumber.Length).Select(_ => new Char(CharType.Number, fullNumber, fullNumber.Length, i - fullNumber.Length)));
                    continue;
                } 
                else if (c.ToString() == ".")
                {
                    lineChars.Add(new Char(CharType.Dot, c.ToString(), 1, i));
                }
                else 
                {
                    lineChars.Add(new Char(CharType.Symbol, c.ToString(), 1, i));
                }
                i++;
            }
            return lineChars;
        }).ToList();
    }

    public bool HasSymbol(List<Char> line, int index, int length) 
    {
        var start = index;
        if (index > 0) 
        {
            start = start - 1;
            length++;
        }
        if (index + length < line.Count -1) 
        {
            length++;
        }
        var slice = line.Slice(start, length);
        if (slice.Any(c => c.type == CharType.Symbol))
        {
            return true;
        }
        return false;
    }

    public bool IsPartNumber(List<List<Char>> lines, int x, int y, Char c)
    {
        // check prev line
        if (x > 0) {
            if (HasSymbol(lines[x - 1], y, c.length))
            {
                return true;
            }
        }
        
        // check current line
        var prev = y > 0 ? lines[x][y -1] : null;
        var next = y + c.length < lines[x].Count ? lines[x][y + c.length] : null;
        if ((prev != null && prev.type == CharType.Symbol) ||
            (next != null && next.type == CharType.Symbol)) 
        {
            return true;
        }

        // check next line
        if (x < lines.Count - 1) {
            if (HasSymbol(lines[x + 1], y, c.length))
            {
                return true;
            }
        }
        return false;
    }

    public override string Part1()
    {
        var lines = ParseInput();
        var partNumbers = new List<int>();
        
        for (var x = 0; x < lines.Count; x++)
        {
            var line = lines[x];
            for (var y = 0; y < line.Count;)
            {
                var c = line[y];
                switch(c.type) 
                {
                    case CharType.Number:
                    {
                        if (IsPartNumber(lines, x, y, c)) {
                            partNumbers.Add(int.Parse(c.value));
                        }
                        y += c.length;
                        break;
                    }

                    default:
                        y++;
                        break;
                }
            }
        }

        return partNumbers.Sum().ToString();
    }

    public List<Char> GetUniqueNumbers(List<Char> line, int index, int length) 
    {
        var start = index;
        if (index > 0) 
        {
            start = start - 1;
            length++;
        }
        if (index + length < line.Count -1) 
        {
            length++;
        }
        var slice = line.Slice(start, length);

        return slice
            .Where(c => c.type == CharType.Number)
            .GroupBy(c => c.index)
            .ToDictionary(g => g.Key, g => g.First())
            .Values
            .ToList();
    }

    public List<Char> GetAdjecentNumbers(List<List<Char>> lines, int x, int y, Char c)
    {
        var adjecentNumbers = new List<Char>();
        // check prev line
        if (x > 0) {
            adjecentNumbers.AddRange(GetUniqueNumbers(lines[x - 1], y, c.length));
        }
        
        // check current line
        var prev = y > 0 ? lines[x][y -1] : null;
        var next = y + c.length < lines[x].Count ? lines[x][y + c.length] : null;
        if (prev != null && prev.type == CharType.Number)
        {
            adjecentNumbers.Add(prev);
        }
        if (next != null && next.type == CharType.Number)
        {
            adjecentNumbers.Add(next);
        }

        // check next line
        if (x < lines.Count - 1) {
            adjecentNumbers.AddRange(GetUniqueNumbers(lines[x + 1], y, c.length));
        }
        return adjecentNumbers;
    }

    public int GetGearValue(List<List<Char>> lines, int x, int y, Char c)
    {
        var adjecentNumers = GetAdjecentNumbers(lines, x, y, c);

        return adjecentNumers.Count() == 2
            ? int.Parse(adjecentNumers.First().value) * int.Parse(adjecentNumers.Last().value)
            : 0;
    }



    public override string Part2()
    {
        var lines = ParseInput();
        var gears = new List<int>();
        
        for (var x = 0; x < lines.Count; x++)
        {
            var line = lines[x];
            for (var y = 0; y < line.Count;)
            {
                var c = line[y];
                if (c.type == CharType.Symbol && c.value == "*"){
                    var gearValue = GetGearValue(lines, x, y, c);
                    if (gearValue > 0) {
                        gears.Add(gearValue);
                    }
                }
                y++;
            }
        }

        return gears.Sum().ToString();
    }
}
