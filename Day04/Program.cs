using Day04;
var input = File.ReadAllLines("input.txt").ToList();
long ret1 = 0;

foreach (var line in input)
{
    var l = line.Substring(line.IndexOf(": ") + 2);
    var sp = l.Split(" | ");

    var winning = sp[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim())).ToList();
    var mine = sp[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim())).ToList();

    var i = winning.Intersect(mine).Count();
    if (i > 0)
    {
        ret1 += (long)Math.Pow(2, i - 1);
    }
}

Dictionary<long, long> cardNums = new Dictionary<long, long>();
for(int i = 0; i <  input.Count; i++)
{
    cardNums.Add(i, 1);
}

long lc = 0;
foreach (var line in input)
{
    var l = line.Substring(line.IndexOf(": ") + 2);
    var sp = l.Split(" | ");

    var winning = sp[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim())).ToList();
    var mine = sp[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim())).ToList();

    var wcn = winning.Intersect(mine).Count();
    for (long i = 0; i < wcn; i++)
    {
        if (lc + i + 1 < cardNums.Count)
            cardNums[lc + i + 1] += cardNums[lc];
    }
    lc++;
}

var ret2 = cardNums.Sum(k => k.Value);
Console.ReadLine();