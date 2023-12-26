using AOCHelper;

var input = File.ReadAllLines("input.txt").ToList();
var map = Helper.ParseInput(input);
map[65][65].ValueCh = '.';

var allTiles = map.SelectMany(kvp => kvp.Value.Values).ToList();

Fill(map, allTiles, new Coordinate { X = 65, Y = 65 });
var fullTableParity0 = allTiles.Count(t => t.DistFromStart % 2 == 0);
var fullTableParity1 = allTiles.Count(t => t.DistFromStart % 2 == 1);

Fill(map, allTiles, new Coordinate { X = 65, Y = 130 });
var up = allTiles.Count(t => t.DistFromStart < 131 && t.DistFromStart % 2 == 0);
Fill(map, allTiles, new Coordinate { X = 65, Y = 0 });
var down = allTiles.Count(t => t.DistFromStart < 131 && t.DistFromStart % 2 == 0);
Fill(map, allTiles, new Coordinate { X = 130, Y = 65 });
var left = allTiles.Count(t => t.DistFromStart < 131 && t.DistFromStart % 2 == 0);
Fill(map, allTiles, new Coordinate { X = 0, Y = 65 });
var right = allTiles.Count(t => t.DistFromStart < 131 && t.DistFromStart % 2 == 0);

Fill(map, allTiles, new Coordinate { X = 130, Y = 130 });
var upleftParity0 = allTiles.Count(t => t.DistFromStart < 65 && t.DistFromStart % 2 == 0);
var upleftParity1 = allTiles.Count(t => t.DistFromStart < 131 + 65 && t.DistFromStart % 2 == 1);
Fill(map, allTiles, new Coordinate { X = 0, Y = 130 });
var uprightParity0 = allTiles.Count(t => t.DistFromStart < 65 && t.DistFromStart % 2 == 0);
var uprightParity1 = allTiles.Count(t => t.DistFromStart < 131 + 65 && t.DistFromStart % 2 == 1);

Fill(map, allTiles, new Coordinate { X = 130, Y = 0 });
var downleftParity0 = allTiles.Count(t => t.DistFromStart < 65 && t.DistFromStart % 2 == 0);
var downleftParity1 = allTiles.Count(t => t.DistFromStart < 131 + 65 && t.DistFromStart % 2 == 1);
Fill(map, allTiles, new Coordinate { X = 0, Y = 0 });
var downrightParity0 = allTiles.Count(t => t.DistFromStart < 65 && t.DistFromStart % 2 == 0);
var downrightParity1 = allTiles.Count(t => t.DistFromStart < 131 + 65 && t.DistFromStart % 2 == 1);

long thenum = 26501365 / 131;

decimal ret2 = fullTableParity1 + 
    up + down + left + right + 
    thenum * (uprightParity0 + downrightParity0 + downleftParity0 + upleftParity0) +
    (thenum - 1) * (uprightParity1 + downrightParity1 + downleftParity1 + upleftParity1);

for (decimal i = 1; i < thenum; i++)
{
    if(i % 2 == 0)
    {
        ret2 += 4 * i * fullTableParity1;
    }
    else
    {
        ret2 += 4 * i * fullTableParity0;
    }
}

Console.ReadLine();

void Fill(Dictionary<long, Dictionary<long, Item>> map, List<Item> allTiles, Coordinate start)
{
    HashSet<Item> fullyKnownItems = new HashSet<Item>();
    //reset
    foreach(var t in allTiles.Where(t=>t.DistFromStart != null))
    {
        t.DistFromStart = null;
    }

    map[start.Y][start.X].DistFromStart = 0;

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