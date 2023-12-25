using AOCHelper;

var input = File.ReadAllLines("input.txt").ToList();
var map = Helper.ParseInput(input);
var allTiles = map.SelectMany(k => k.Value.Values).ToList();

var startPos = map[0].Values.Single(t => t.ValueCh == '.');
var endPos = map[map.Count - 1].Values.Single(t => t.ValueCh == '.');
List<Path> possiblePathes = new List<Path>();
List<Path> completedPathes = new List<Path>();
var JunctionCache = new Dictionary<Item, List<(int, Item)>>();

var fj = FindNext(map, startPos, startPos);
BuildJunctionCache(map);
var lastmax = 0;

Search(JunctionCache[fj.Item2], fj.Item1 - 1, new HashSet<string> { startPos.Id, fj.Item2.Id });

var ret2 = lastmax;

Console.ReadLine();

void BuildJunctionCache(Dictionary<long, Dictionary<long, Item>> map)
{
    var dis = new List<Item> { startPos };

    while (dis.Any())
    {
        var cdi = dis.First();
        JunctionCache.Add(cdi, new List<(int, Item)>());
        var pps = Helper.AdjacentPossiblePlaces(map, cdi, new List<Item> { }, null);

        foreach (var p in pps)
        {
            var a = FindNext(map, cdi, p.Item2);
            JunctionCache[cdi].Insert(0, a);

            if (!JunctionCache.ContainsKey(a.Item2) && !dis.Contains(a.Item2))
            {
                dis.Add(a.Item2);
            }
        }
        dis = dis.Skip(1).ToList();
    }
}

(int, Item) FindNext(Dictionary<long, Dictionary<long, Item>> map, Item lastPos, Item currPos)
{
    var pps = Helper.AdjacentPossiblePlaces(map, currPos, new List<Item>(), lastPos);
    var retCount = 1;
    while (pps.Count == 1)
    {
        var opps = pps.Single();
        var cops = currPos;
        currPos = opps.Item2;
        pps = Helper.AdjacentPossiblePlaces(map, currPos, new List<Item>(), cops);
        retCount++;
        if (currPos == endPos || currPos == startPos)
        {
            break;
        }
    }
    return (retCount, currPos);
}

void Search(List<(int, Item)> curr, int stepcount, HashSet<string> visitedNodes)
{
    foreach(var e in curr)
    {
        if (!visitedNodes.Contains(e.Item2.Id))
        {
            if (e.Item2 == endPos)
            {
                if (lastmax < stepcount + e.Item1)
                {
                    lastmax = stepcount + e.Item1;
                }
                return;
            }
            else
            {
                visitedNodes.Add(e.Item2.Id);
                Search(JunctionCache[e.Item2], stepcount + e.Item1, visitedNodes);
                visitedNodes.Remove(e.Item2.Id);
            }
        }
    }
}

class Path
{
    public HashSet<string> Junctions { get; set; } = new HashSet<string>();
    //public Dictionary<Item, int> JunctionSteps { get; set; } = new Dictionary<Item, int>();
    public Item LastStep { get; set; }
    public int StepCount { get; set; } = 0;
}