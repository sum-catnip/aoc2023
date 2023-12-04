Stream file = File.OpenRead("input.txt");
var reader = new StreamReader(file);

int part1 = 0;
var copies = new Dictionary<int, int>();
while(!reader.EndOfStream)
{
    Card card = ParseCard(reader);
    var intersections = card.ActualNums.Intersect(card.WinNums).Count();
    part1 += (int)Math.Pow(2, intersections - 1);

    copies[card.Id] = copies.GetValueOrDefault(card.Id, 0) + 1;
    // get copy count of current card
    foreach(var _ in Enumerable.Range(0, copies[card.Id]))
    {
        // generate copies
        foreach (int i in Enumerable.Range(card.Id +1, intersections))
        {
            // increase copy count by one
            int curr = copies.GetValueOrDefault(i, 0);
            copies[i] = curr + 1;
        }
    }
}

foreach(var (k, v) in copies)
{
    Console.WriteLine($"card {k} has {v} copies");
}

Console.WriteLine($"part1 solution: {part1}");
Console.WriteLine($"part2 solution: {copies.Values.Sum()}");

string ReadString(StreamReader reader, int num)
{
    var data = new char[num];
    reader.Read(data.AsSpan());
    return new string(data);
}

int ReadNum(StreamReader reader)
{
    var digits = new List<char>();
    // skip whitespace
    while(reader.Peek() is int c && c > -1 && c == ' ')
    {
        reader.Read();
    }
    while(reader.Peek() is int c && c > -1 && char.IsDigit((char)c))
    {
        digits.Add((char)reader.Read());
    }

    return int.Parse(digits.ToArray());
}

Card ParseCard(StreamReader reader)
{
    // get rid of Card
    ReadString(reader, "Card ".Length);
    int id = ReadNum(reader);
    ReadString(reader, ": ".Length);

    // read winning numbers
    var winning = new List<int>();
    while (reader.Peek() is int c && c > -1 && c != '|')
    {
        winning.Add(ReadNum(reader));
        ReadString(reader, " ".Length);
    }

    ReadString(reader, " ".Length);

    // read actual numbers
    var actual = new List<int>();
    while (reader.Peek() is int c && c > -1)
    {
        actual.Add(ReadNum(reader));
        if (ReadString(reader, 1) == "\n") break;
    }

    return new() { Id = id, ActualNums = actual, WinNums = winning };
}

struct Card
{
    public int Id { get; set;}
    public List<int> WinNums { get; set;}
    public List<int> ActualNums { get; set;}
}