var input = File.ReadAllLines("input.txt").ToList();
long ret1 = 0;

var c = 0;
long ret2 = 0;
foreach (var line in input)
{
    var sp = line.Split(" ");
    var damagedStr = sp[1].Split(",");
    var damageds = damagedStr.Select(d => int.Parse(d)).ToList();
    var layout = sp[0] + "?" + sp[0] + "?" + sp[0] + "?" + sp[0] + "?" + sp[0];
    damageds = damageds.Concat(damageds).Concat(damageds).Concat(damageds).Concat(damageds).ToList();

    long sr = 0;
    long ss = 0;
    var ca = new Dictionary<string, Dictionary<int, Dictionary<int, long>>>();
    Solve(layout, 0, 0, damageds, ref ss, ref sr, ca, "");
    ret2 += sr;
}


Console.ReadLine();

void Solve(string layout, int pos, int currentGroupSize, List<int> damageds, ref long subSeqNum, ref long result, Dictionary<string, Dictionary<int, Dictionary<int, long>>> cache, string debug)
{
    var ok = layout.Substring(pos);
    if (cache.ContainsKey(ok) && cache[ok].ContainsKey(damageds.Count) && cache[ok][damageds.Count].ContainsKey(currentGroupSize))
    {
        subSeqNum += cache[ok][damageds.Count][currentGroupSize];
        result += cache[ok][damageds.Count][currentGroupSize];
        return;
    }

    if (pos == layout.Length)
    {
        if ((!damageds.Any() && currentGroupSize == 0) || (damageds.Count == 1 && currentGroupSize == damageds.Single()))
        {
            ret1 += 1;
            subSeqNum += 1;
            result += 1;
        }
        return;
    }

    if (layout[pos] == '#')
    {
        long ss = 0;
        var k = layout.Substring(pos + 1);
        if (!cache.ContainsKey(k))
            cache.Add(k, new Dictionary<int, Dictionary<int, long>>());
        if (!cache[k].ContainsKey(damageds.Count))
        {
            cache[k].Add(damageds.Count, new Dictionary<int, long>());
        }

        Solve(layout, pos + 1, currentGroupSize + 1, damageds, ref ss, ref result, cache, debug + layout[pos]);
        subSeqNum += ss;
        if (!cache[k][damageds.Count].ContainsKey(currentGroupSize + 1))
        {
            cache[k][damageds.Count].Add(currentGroupSize + 1, ss);
        }
    }
    else if (layout[pos] == '.')
    {
        if (currentGroupSize == 0)
        {
            long ss = 0;
            var k = layout.Substring(pos + 1);
            if (!cache.ContainsKey(k))
                cache.Add(k, new Dictionary<int, Dictionary<int, long>>());
            if (!cache[k].ContainsKey(damageds.Count))
            {
                cache[k].Add(damageds.Count, new Dictionary<int, long>());
            }
            Solve(layout, pos + 1, 0, damageds, ref ss, ref result, cache, debug + layout[pos]);
            subSeqNum += ss;
            if (!cache[k][damageds.Count].ContainsKey(0))
            {
                cache[k][damageds.Count].Add(0, ss);
            }
        }
        else if (damageds.Any() && damageds.First() == currentGroupSize)
        {
            long ss = 0;
            var k = layout.Substring(pos + 1);
            if (!cache.ContainsKey(k))
                cache.Add(k, new Dictionary<int, Dictionary<int, long>>());
            if (!cache[k].ContainsKey(damageds.Count - 1))
            {
                cache[k].Add(damageds.Count - 1, new Dictionary<int, long>());
            }

            Solve(layout, pos + 1, 0, damageds.Skip(1).ToList(), ref ss, ref result, cache, debug + layout[pos]);
            subSeqNum += ss;
            if (!cache[k][damageds.Count - 1].ContainsKey(0))
            {
                cache[k][damageds.Count - 1].Add(0, ss);
            }
        }
        else
        {
            return;
        }
    }
    else
    {
        if (damageds.Any() && currentGroupSize + 1 <= damageds.First())
        {
            long ss = 0;
            var k = layout.Substring(pos + 1);
            if (!cache.ContainsKey(k))
                cache.Add(k, new Dictionary<int, Dictionary<int, long>>());
            if (!cache[k].ContainsKey(damageds.Count))
            {
                cache[k].Add(damageds.Count, new Dictionary<int, long>());
            }

            Solve(layout, pos + 1, currentGroupSize + 1, damageds, ref ss, ref result, cache, debug + "#");
            subSeqNum += ss;
            if (!cache[k][damageds.Count].ContainsKey(currentGroupSize + 1))
            {
                cache[k][damageds.Count].Add(currentGroupSize + 1, ss);
            }
        }

        if (currentGroupSize == 0 || (damageds.Any() && currentGroupSize == damageds.First()))
        {
            long ss = 0;
            var xd = (currentGroupSize == 0) ? 0 : 1;
            var k = layout.Substring(pos + 1);
            if (!cache.ContainsKey(k))
                cache.Add(k, new Dictionary<int, Dictionary<int, long>>());
            if (!cache[k].ContainsKey(damageds.Count - xd))
            {
                cache[k].Add(damageds.Count - xd, new Dictionary<int, long>());
            }

            Solve(layout, pos + 1, 0, (currentGroupSize == 0) ? damageds : damageds.Skip(1).ToList(), ref ss, ref result, cache, debug + ".");
            subSeqNum += ss;
            if (!cache[k][damageds.Count - xd].ContainsKey(currentGroupSize))
            {
                cache[k][damageds.Count - xd].Add(currentGroupSize, ss);
            }
        }
    }
}
