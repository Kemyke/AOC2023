using AOCHelper;
using System.ComponentModel;
using System.Data;
var s = String.Format("|{0,5}|{1,5}|{2,5}", new List<object> { 1, "#", 300 }.ToArray());

//var t = Predict(new List<long> { 3819, 33769, 94549, 184399, 305959 });
305959

var input = File.ReadAllLines("input.txt").ToList();
var biginput = new List<string>();
foreach(var l in input)
{
    var rl = l.Replace("S", ".");
    biginput.Add(rl + rl + rl + rl + rl + rl + rl + rl + rl + rl + rl);
}

biginput = biginput.Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).Concat(biginput).ToList();

var map = Helper.ParseInput(biginput);

var orig = Helper.ParseInput(input);
var origStart = orig.SelectMany(kvp => kvp.Value.Values).Single(i => i.ValueCh == 'S').Coordinate;
var sx = 5 * 131 + origStart.X;
var sy = 5 * 131 + origStart.Y;

var allTiles = map.SelectMany(kvp => kvp.Value.Values).ToList();
var start = allTiles.Single(i => i.Coordinate.X == sx && i.Coordinate.Y == sy);
start.ValueCh = 'S';
Helper.VisualizeMapFile(map);
var startingPos = start.Coordinate;
start.ValueCh = '.';
start.DistFromStart = 0;

//for (int i = 1; i < 5; i++)
//{
//    AddNewPart(-i, -i, map, orig);
//    AddNewPart(i, i, map, orig);
//    AddNewPart(i, -i, map, orig);
//    AddNewPart(-i, i, map, orig);
//    var d = 1;
//    for(int j = -i + 1; j < i; j++)
//    {
//        AddNewPart(-i, -i + d, map, orig);
//        AddNewPart(i, -i + d, map, orig);

//        AddNewPart(-i + d, -i, map, orig);
//        AddNewPart(-i + d, i, map, orig);
//        d++;
//    }
//}
//allTiles = map.SelectMany(kvp => kvp.Value.Values).ToList();

Fill(map, allTiles);
Helper.VisualizeMapInFile(map, 131, 131);
var rts1 = allTiles.Count(t => t.DistFromStart % 2 == 1 && t.DistFromStart <= 65);
var rts2 = allTiles.Count(t => t.DistFromStart % 2 == 1 && t.DistFromStart <= 196);
var rts3 = allTiles.Count(t => t.DistFromStart % 2 == 1 && t.DistFromStart <= 327);
var rts4 = allTiles.Count(t => t.DistFromStart % 2 == 1 && t.DistFromStart <= 458);
var rts5 = allTiles.Count(t => t.DistFromStart % 2 == 1 && t.DistFromStart <= 589);
Console.WriteLine(rts1);
Console.WriteLine(rts2);
Console.WriteLine(rts3);
Console.WriteLine(rts4);
Console.WriteLine(rts5);

//141 vertical
//145 horizontal

Console.ReadLine();
//3819
//33664
//94183
//183578
//304691

//3819
//33769
//94549
//184399

long Predict(List<long> nums)
{
    var cns = new List<List<long>> { nums };
    var cn = nums.ToList();
    while (!cn.All(n => n == 0))
    {
        var nl = new List<long>();
        for (int i = 1; i < cn.Count; i++)
        {
            nl.Add(cn[i] - cn[i - 1]);
        }

        cns.Add(nl);
        cn = nl;
    }

    long lastDiff = 0;
    for (var i = cns.Count - 1; i >= 0; i--)
    {
        cns[i].Add(cns[i].Last() + lastDiff);
        lastDiff = cns[i].Last();
    }
    return cns[0].Last();

}

void AddNewPart(long vnum, long hnum, Dictionary<long, Dictionary<long, Item>> map, Dictionary<long, Dictionary<long, Item>> orig)
{
    int j = 0;
    var ymax = orig.Count;
    var xmax = orig[0].Count;
    foreach (var line in orig)
    {
        var y = vnum * ymax + j;
        if(!map.ContainsKey(y))
            map.Add(y, new Dictionary<long, Item>());
        foreach(var item in line.Value)
        {
            var ni = item.Value.Clone();
            var x = hnum * xmax + ni.Coordinate.X;
            ni.Coordinate.Y = y;
            ni.Coordinate.X = x;
            map[y].Add(x, ni);
        }
        j++;
    }
}

void Fill(Dictionary<long, Dictionary<long, Item>> map, List<Item> allTiles)
{
    HashSet<Item> fullyKnownItems = new HashSet<Item>();

    while (allTiles.Any(t => t.ValueCh == '.' && t.DistFromStart == null))
    {
        var kts = allTiles.Where(t => t.DistFromStart != null && !fullyKnownItems.Contains(t)).ToList();
        if (!kts.Any())
        {
            var wtf = allTiles.Where(t => t.ValueCh == '.' && t.DistFromStart == null).ToList();
            break;
        }
        foreach (var kt in kts)
        {
            var nbs = Helper.AdjacentItems(map, kt).Where(t => t.ValueCh == '.' && t.DistFromStart == null).ToList();
            if (!nbs.Any())
            {
                fullyKnownItems.Add(kt);
            }
            else
            {
                foreach (var nb in nbs)
                {
                    nb.DistFromStart = kt.DistFromStart + 1;
                }
            }
        }
    }
}
