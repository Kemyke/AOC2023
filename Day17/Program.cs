using AOCHelper;

var input = File.ReadAllLines("input.txt").ToList();
var map = Helper.ParseInput(input);
List<AOCPath> possiblePathes = new List<AOCPath>();
Dictionary<long, Dictionary<long, int>> cache = new Dictionary<long, Dictionary<long, int>>();

AOCPath cp = new AOCPath { CurrentPos = map[0][0], HeatLoss = 0, Path = new List<Item> { map[0][0] }, DirCount = 0, LastDir = "" };
possiblePathes.Add(cp);
while(cp.CurrentPos.Coordinate.X != map[0].Count - 1 || cp.CurrentPos.Coordinate.Y != map.Count - 1)
{
    Step(map, cp);
    possiblePathes = possiblePathes.Skip(1).OrderBy(k=>k.HeatLoss).ToList();
    cp = possiblePathes.First();
}
Console.ReadLine();

void Step(Dictionary<long, Dictionary<long, Item>> map, AOCPath f)
{
    var possItems = Helper.PossibleItems(map, f);

    foreach (var pi in possItems)
    {
        var np = new AOCPath
        {
            HeatLoss = f.HeatLoss + pi.Item2.Value,
            CurrentPos = pi.Item2,
            Path = f.Path.Concat(new List<Item> { pi.Item2 }).ToList(),
            DirCount = (pi.Item1 == f.LastDir) ? f.DirCount + 1 : 1,
            LastDir = pi.Item1,
            PathStr = f.PathStr + pi.Item1,
        };

        if (!pi.Item2.MinHeatLosses.ContainsKey(pi.Item1) || (!pi.Item2.MinHeatLosses[pi.Item1].ContainsKey(np.DirCount)) ||
              pi.Item2.MinHeatLosses[pi.Item1][np.DirCount] > np.HeatLoss)
        {
            if (!pi.Item2.MinHeatLosses.ContainsKey(pi.Item1))
                pi.Item2.MinHeatLosses.Add(pi.Item1, new Dictionary<int, int>());
            pi.Item2.MinHeatLosses[pi.Item1][np.DirCount] = np.HeatLoss;
            possiblePathes.Add(np);
        }
    }
}