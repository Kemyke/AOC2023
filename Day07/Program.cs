var input = File.ReadAllLines("input.txt");

List<Hand> hands = input.Select(l=>Hand.Parse(l)).ToList();
var ordered = hands.Order(new Hand()).ToList();
long ret2 = 0;

for(int i = 1; i < ordered.Count + 1; i++)
{
    ret2 += i * ordered[i - 1].Bet;
}

Console.ReadLine();

class Hand : IComparer<Hand>
{
    public string HandStr { get; set; }
    public long Bet { get; set; }
    public int Strongness { get; set; }

    public int Compare(Hand? x, Hand? y)
    {
        if (x.Strongness != y.Strongness)
            return x.Strongness - y.Strongness;
        for(int i = 0; i < 5; i++)
        {
            if (x.HandStr[i] != y.HandStr[i])
                return x.HandStr[i] - y.HandStr[i];
        }
        return 0;
    }

    public static Hand Parse(string hand)
    {
        var sp = hand.Split(" ");
        var cs = sp[0].Distinct().ToDictionary(k => k.ToString(), v => sp[0].Count(c => c == v));
        var ret = new Hand { Bet = long.Parse(sp[1]), HandStr = sp[0].Replace("A", "Z").Replace("K", "Y").Replace("Q", "X").Replace("J", "0") };

        var jokers = cs.ContainsKey("J") ? cs["J"] : 0;
        cs.Remove("J");
        if (jokers == 0)
        {
            ret.Strongness = cs.Count(k => k.Value == 5) == 1 ? 7 : -1;
            if (ret.Strongness == -1)
            {
                ret.Strongness = cs.Count(k => k.Value == 4) == 1 ? 6 : -1;
            }
            if (ret.Strongness == -1)
            {
                ret.Strongness = cs.Count(k => k.Value == 3) == 1 && cs.Count(k => k.Value == 2) == 1 ? 5 : -1;
            }
            if (ret.Strongness == -1)
            {
                ret.Strongness = cs.Count(k => k.Value == 3) == 1 ? 4 : -1;
            }
            if (ret.Strongness == -1)
            {
                ret.Strongness = cs.Count(k => k.Value == 2) == 2 ? 3 : -1;
            }
            if (ret.Strongness == -1)
            {
                ret.Strongness = cs.Count(k => k.Value + jokers == 2) == 1 ? 2 : -1;
            }
            if (ret.Strongness == -1)
            {
                ret.Strongness = 1;
            }
        }
        else
        {
            if (jokers == 5 || jokers == 4) { ret.Strongness = 7; }
            else if (jokers == 3) { if (cs.Count == 1) { ret.Strongness = 7; } else { ret.Strongness = 6; } }
            else if (jokers == 2) { if (cs.Count == 3) { ret.Strongness = 4; } else if (cs.Count == 2) { ret.Strongness = 6; } else if (cs.Count == 1) { ret.Strongness = 7; } }
            else if (jokers == 1)
            {
                if (cs.Max(kvp => kvp.Value) == 4)
                    ret.Strongness = 7;
                else if (cs.Max(kvp => kvp.Value) == 3)
                    ret.Strongness = 6;
                else if (cs.Count(k => k.Value == 2) == 2)
                    ret.Strongness = 5;
                else if (cs.Count(k => k.Value == 2) == 1)
                    ret.Strongness = 4;
                else
                    ret.Strongness = 2;

            }
        }
        return ret;
    }
}