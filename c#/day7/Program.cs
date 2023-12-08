var turns = File
    .ReadLines("input.txt")
    .Select(l => l.Split(' '))
    .Select((pair) => new Turn(pair[0], int.Parse(pair[1])))
    .OrderByDescending(turn => turn.hand, Comparer<string>.Create(CompareHands))
    .Select((t, i) => t.bid * (i +1))
    ;

Console.WriteLine($"part1 solution: {turns.Sum()}");

IEnumerable<(char character, int count)> CountSameKind(string hand) =>
    hand
        .GroupBy(c => c)
        .Select(group => (x: group.Key, count: group.Count()))
        .OrderByDescending(pair => pair.count);

HandType DetermineHandType(string hand)
{
    var groups = CountSameKind(hand).ToArray();
    return hand switch
    {
        string _ when groups.First().count == 5 => HandType.FiveOfKind,
        string _ when groups.First().count == 4 => HandType.FourOfKind,
        string _ when groups.First().count == 3 && groups[1].count == 2 => HandType.FullHouse,
        string _ when groups.First().count == 3 => HandType.ThreeOfKind,
        string _ when groups.First().count == 2 && groups[1].count == 2 => HandType.TwoPair,
        string _ when groups.First().count == 2 => HandType.OnePair,
        _ => HandType.HighCard
    };
}

CardType DetermineCardType(char card) =>
    card switch
    {
        'A' => CardType.A,
        'K' => CardType.K,
        'Q' => CardType.Q,
        'J' => CardType.J,
        'T' => CardType.T,
        >= '2' and <= '9' => (CardType)card - '0'
    };

int CompareHands(string h1, string h2) => 
    (DetermineHandType(h1) - DetermineHandType(h2)) switch
    {
        0 => h1.Zip(h2)
            .Select(pair => DetermineCardType(pair.Second) - DetermineCardType(pair.First))
            .SkipWhile(x => x == 0)
            .FirstOrDefault(),
        int x => x
    };

record Turn(string hand, int bid);
enum HandType { FiveOfKind = 1, FourOfKind, FullHouse, ThreeOfKind, TwoPair, OnePair, HighCard }
enum CardType { _2 = 2, _3, _4, _5, _6, _7, _8, _9, T, J, Q, K, A }