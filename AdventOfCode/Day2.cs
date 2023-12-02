
namespace AdventOfCode;

public record Cubes(int count, string color); 
public record Set(List<Cubes> cubes);
public record Game(int id, List<Set> sets);

internal class Day2 : Puzzle
{
    private readonly Dictionary<string, int> maxCubes = new Dictionary<string, int> {{ "red", 12}, { "green", 13}, { "blue", 14}};

    public List<Game> ParseInput() {
        return Input.Select(line => {
            var lineParts = line.Split(":");

            return new Game(
                int.Parse(lineParts.First().Split(" ").Last()), 
                lineParts.Last().Split(";").Select(set => {
                    return new Set(set.Split(",").Select(cube => {
                        var cubeParts = cube.Trim().Split(" ");
                        return new Cubes(
                            int.Parse(cubeParts[0].Trim()),
                            cubeParts[1].Trim()
                        );
                    }).ToList());
                }).ToList());
        }).ToList();
    }

    public override string Part1()
    {
        var parsedInput = ParseInput();
        
        var validGames = parsedInput
            .Where(game => !game.sets.Any(set => set.cubes.Any(cube => maxCubes[cube.color] < cube.count)))
            .ToList();
        return validGames.Sum(game => game.id).ToString();
    }
    public override string Part2()
    {
        var parsedInput = ParseInput();

        return parsedInput.Select(game => {
            var red = 0;
            var green = 0; 
            var blue = 0;
            foreach (var set in game.sets) {
                foreach (var cube in set.cubes) {
                    switch(cube.color) {
                        case "red": red = red < cube.count ? cube.count : red; break; 
                        case "green": green = green < cube.count ? cube.count : green; break; 
                        case "blue": blue = blue < cube.count ? cube.count : blue; break; 
                    }
                }
            }

            return red * green * blue;
        }).ToList().Sum().ToString();
    }
}
