using System.Diagnostics;
using System.Text.RegularExpressions;


Regex RE = new(@"(?<key>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)", RegexOptions.Compiled);
string[] lines = File.ReadAllLines("input.txt");
string instructions = lines[0];
var map = lines
    .Skip(2)
    .Select(line => RE.Match(line).Groups)
    .ToDictionary(g => g["key"].Value, g => (left: g["left"].Value, right: g["right"].Value));

int steps = 0;
string key = "AAA";
while (key != "ZZZ")
{
    foreach(char c in instructions)
    {
        key = c switch
        {
            'L' => map[key].left,
            'R' => map[key].right
        };
        steps++;
    }
}

Console.WriteLine($"part1: {steps}");
