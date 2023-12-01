string[] mapping = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

using Stream file = File.OpenRead("input.txt");
using StreamReader reader = new(file);
int sum = 0;

int LookupNum(string num) => Array.IndexOf(mapping, num) % 9 + 1;
while (reader.ReadLine() is string line)
{
    // i could deduplicate this linq by parametrizing the IndexOf func
    // but the code is already complicated enough as-is
    var (_, first) = mapping
        .Select(num => (index: line.IndexOf(num), number: LookupNum(num)))
        .Where(pair => pair.index > -1)
        .OrderBy(pair => pair.index)
        .First();

    var (_, last) = mapping
        .Select(num => (index: line.LastIndexOf(num), number: LookupNum(num)))
        .Where(pair => pair.index > -1)
        .OrderBy(pair => pair.index)
        .Last();

    sum += int.Parse($"{first}{last}");
}

Console.WriteLine(sum);
