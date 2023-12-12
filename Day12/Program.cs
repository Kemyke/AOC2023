var input = File.ReadAllLines("input.txt").ToList();
long ret1 = 0;

var c = 0;
foreach (var line in input)
{
    var sp = line.Split(" ");
    var damagedStr = sp[1].Split(",");
    var damaged = damagedStr.Select(d => int.Parse(d)).ToList();
    var layout = sp[0] + "?" + sp[0] + "?" + sp[0] + "?" + sp[0] + "?" + sp[0];
    damaged = damaged.Concat(damaged).Concat(damaged).Concat(damaged).Concat(damaged).ToList();

    //Console.WriteLine(sp[1]);
    //Console.WriteLine(sp[0]);
    Solve(layout, 0, 0, damaged, "");
    c++;
    Console.WriteLine(c);
    //Console.WriteLine();
    //Console.WriteLine();
    //Console.WriteLine();
    //Console.WriteLine();
}

//7619 too high

Console.ReadLine();

void Solve(string layout, int pos, int currentGroupSize, List<int> damageds, string debug)
{
    while (pos < layout.Length && layout[pos] != '?')
    {
        if (layout[pos] == '#')
        {
            currentGroupSize += 1;
        }
        else if (layout[pos] == '.')
        {
            if (currentGroupSize == 0)
            {

            }
            else if (damageds.Any() && damageds.First() == currentGroupSize)
            {
                currentGroupSize = 0;
                damageds = damageds.Skip(1).ToList();
            }
            else
            {
                return;
            }
        }
        debug += layout[pos];
        pos++;
    }

    if (pos == layout.Length)
    {
        if ((!damageds.Any() && currentGroupSize == 0) || (damageds.Count == 1 &&currentGroupSize == damageds.Single()))
        {
            ret1 += 1;
            //Console.WriteLine(debug);
        }
        return;
    }

    if (damageds.Any() && currentGroupSize + 1 <= damageds.First())
    {
        Solve(layout, pos + 1, currentGroupSize + 1, damageds, debug + "#");
    }
    
    if (currentGroupSize == 0 || (damageds.Any() && currentGroupSize == damageds.First()))
    {
        Solve(layout, pos + 1, 0, (currentGroupSize == 0) ? damageds : damageds.Skip(1).ToList(), debug + ".");
    }
}
