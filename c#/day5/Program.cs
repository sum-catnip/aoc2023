Stream fs = File.OpenRead("input.txt");
var reader = new StreamReader(fs);

long[] ParseIntArray(string str)
    => str.Trim().Split(' ').Select(long.Parse).ToArray();

IEnumerable<Mapping> ReadMap(StreamReader reader)
{
    while (reader.ReadLine() is string sts && sts.Length > 0)
    {
        var nums = ParseIntArray(sts);
        var dst = new Range(nums[0], nums[0] + nums[2]);
        var src = new Range(nums[1], nums[1] + nums[2]);
        yield return new Mapping(src, dst);
    }
}

IEnumerable<IEnumerable<Mapping>> ReadMaps(StreamReader reader)
{
    while (reader.ReadLine() is string line)
    {
        yield return ReadMap(reader);
    }
}

long MapAB(long value, Mapping mapping)
{
    if (mapping.Src.Start <= value && mapping.Src.End > value)
    {
        long diff = value - mapping.Src.Start;
        return mapping.Dst.Start + diff;
    }

    return value;
}

string line = reader.ReadLine();
long[] seeds = ParseIntArray(line.Split(':')[1]);

reader.ReadLine();

foreach(var map in ReadMaps(reader))
{
    Console.WriteLine();
    Console.WriteLine("new map --");
    long[] linenums = (long[])seeds.Clone();
    foreach(var mapping in map)
    {
        for (long i = 0; i <  linenums.Length; i++)
        {
            long mapped = MapAB(linenums[i], mapping);
            if (mapped != linenums[i])
            {
                seeds[i] = mapped;
            }
        }
        Console.WriteLine(mapping);
        Console.WriteLine(string.Join(" ", seeds));
    }
}

Console.WriteLine($"shortest location: {seeds.Min()}");

record Mapping(Range Src, Range Dst);
record Range(long Start, long End);
