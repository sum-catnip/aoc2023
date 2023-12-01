string[] mapping = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

using Stream file = File.OpenRead("input.txt");
using StreamReader reader = new(file);
int sum = 0;

int LookupNum(string num) => Array.IndexOf(mapping, num) % 9 + 1;
while (reader.ReadLine() is string line)
{
    var sorted = mapping
        .Select(num => (index: line.IndexOf(num), number: LookupNum(num)))
        .Concat(mapping.Select(num => (index: line.LastIndexOf(num), number: LookupNum(num))))
        .Where(pair => pair.index > -1)
        .OrderBy(pair => pair.index)
        .ToArray();

    sum += int.Parse($"{sorted.First().number}{sorted.Last().number}");
}

Console.WriteLine(sum);
