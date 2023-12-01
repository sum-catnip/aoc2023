using Stream file = File.OpenRead("input.txt");
using StreamReader reader = new(file);
int sum = 0;
while (reader.ReadLine() is string line)
{
    char first = line.First(char.IsNumber);
    char last = line.Last(char.IsNumber);
    sum += int.Parse($"{first}{last}");
}

Console.WriteLine(sum);
