string[] lines = File.ReadAllLines("input.txt");

int[] ParseInts(string line) => line.Split(':')[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
int Simulate(int time, int windup) => (time - windup) * windup;
int CalculateStrategies((int First, int Second) game) =>
    Enumerable
        .Range(1, game.First - 1)
        .Select(windup => Simulate(game.First, windup))
        .Where(dist => dist > game.Second)
        .Count();

int[] times = ParseInts(lines[0]);
int[] distances = ParseInts(lines[1]);

var margin = times.Zip(distances)
    .Select(CalculateStrategies)
    .Aggregate(1, (acc, x) => acc * x);

Console.WriteLine($"part1 solution: {margin}");
