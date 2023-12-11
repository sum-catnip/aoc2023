
int Extrapolate(int[] items) => items switch
    {
        _ when items.All(x => x == 0) => 0,
        _ => items.Last() + Extrapolate(items
        .Zip(items.Skip(1))
        .Select((pair) => pair.Second - pair.First)
        .ToArray())
    };

// part1
Console.WriteLine(
    File.ReadAllLines("input.txt")
        .Select(l => Extrapolate(l.Split(' ').Select(int.Parse).ToArray()))
        .Sum());

// part2
Console.WriteLine(
    File.ReadAllLines("input.txt")
        .Select(l => Extrapolate(l.Split(' ').Select(int.Parse).Reverse().ToArray()))
        .Sum());
