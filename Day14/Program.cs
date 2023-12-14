using AOCHelper;
using System.Text;

var input = File.ReadAllLines("input.txt").ToList();
var map = Helper.ParseInput(input);

//var moved = TiltToNorth(map);
//while(moved)
//{
//    moved = TiltToNorth(map);
//}

//long ret1 = 0;
//var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O');
//foreach(var rock in roundedRocks)
//{
//    ret1 += (input.Count - rock.Coordinate.Y);
//}

Dictionary<string, (string, int, long, Dictionary<long, Dictionary<long, Item>>)> cachedMoves = new Dictionary<string, (string, int, long, Dictionary<long, Dictionary<long, Item>>)>();

var ck = MapToStr(map);
int n = 0;
for (int i = 0; i < 1000000000; i++)
{
    if (cachedMoves.ContainsKey(ck))
    {
        var ci = cachedMoves[ck];
        map = ci.Item4;
        Console.WriteLine(i+ " "+ci.Item2+ " "+ci.Item3);
    }
    else
    {
        TotalTiltToNorth(map);
        TotalTiltToWest(map);
        TotalTiltToSouth(map);
        TotalTiltToEast(map);
        //Helper.VisualizeMap(map);
        //Console.WriteLine(i);
    }
    if (!cachedMoves.ContainsKey(ck))
    {
        cachedMoves.Add(ck, (MapToStr(map), n++, ComputeRet(map), map));
    }

    ck = cachedMoves[ck].Item1;
}

long ret2 = 0;
var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O');
foreach (var rock in roundedRocks)
{
    ret2 += (input.Count - rock.Coordinate.Y);
}

Console.WriteLine(ret2);
Console.ReadLine();

long ComputeRet(Dictionary<long, Dictionary<long, Item>> map)
{
    long ret = 0;
    var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O');
    foreach (var rock in roundedRocks)
    {
        ret += (input.Count - rock.Coordinate.Y);
    }
    return ret;
}

void TotalTiltToNorth(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = TiltToNorth(map);
    while (moved)
    {
        moved = TiltToNorth(map);
    }
}

void TotalTiltToSouth(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = TiltToSouth(map);
    while (moved)
    {
        moved = TiltToSouth(map);
    }
}

void TotalTiltToWest(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = TiltToWest(map);
    while (moved)
    {
        moved = TiltToWest(map);
    }
}

void TotalTiltToEast(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = TiltToEast(map);
    while (moved)
    {
        moved = TiltToEast(map);
    }
}

bool TiltToNorth(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = false;
    var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O');
    foreach(var rock in roundedRocks)
    {
        if(rock.Coordinate.Y == 0)
        {
            continue;
        }
        else if (map[rock.Coordinate.Y - 1][rock.Coordinate.X].ValueCh == '.')
        {
            map[rock.Coordinate.Y - 1][rock.Coordinate.X].ValueCh = 'O';
            map[rock.Coordinate.Y][rock.Coordinate.X].ValueCh = '.';
            moved = true;
        }
    }
    return moved;
}

bool TiltToSouth(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = false;
    var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O').OrderByDescending(r => r.Coordinate.Y);
    foreach (var rock in roundedRocks)
    {
        if (rock.Coordinate.Y == map.Keys.Count - 1)
        {
            continue;
        }
        else if (map[rock.Coordinate.Y + 1][rock.Coordinate.X].ValueCh == '.')
        {
            map[rock.Coordinate.Y + 1][rock.Coordinate.X].ValueCh = 'O';
            map[rock.Coordinate.Y][rock.Coordinate.X].ValueCh = '.';
            moved = true;
        }
    }
    return moved;
}

bool TiltToWest(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = false;
    var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O').OrderBy(r => r.Coordinate.X);
    foreach (var rock in roundedRocks)
    {
        if (rock.Coordinate.X == 0)
        {
            continue;
        }
        else if (map[rock.Coordinate.Y][rock.Coordinate.X - 1].ValueCh == '.')
        {
            map[rock.Coordinate.Y][rock.Coordinate.X - 1].ValueCh = 'O';
            map[rock.Coordinate.Y][rock.Coordinate.X].ValueCh = '.';
            moved = true;
        }
    }
    return moved;
}

bool TiltToEast(Dictionary<long, Dictionary<long, Item>> map)
{
    var moved = false;
    var roundedRocks = map.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(item => item.ValueCh == 'O').OrderByDescending(r => r.Coordinate.X);
    foreach (var rock in roundedRocks)
    {
        if (rock.Coordinate.X == map[0].Keys.Count - 1)
        {
            continue;
        }
        else if (map[rock.Coordinate.Y][rock.Coordinate.X + 1].ValueCh == '.')
        {
            map[rock.Coordinate.Y][rock.Coordinate.X + 1].ValueCh = 'O';
            map[rock.Coordinate.Y][rock.Coordinate.X].ValueCh = '.';
            moved = true;
        }
    }
    return moved;
}

string MapToStr(Dictionary<long, Dictionary<long, Item>> map)
{
    StringBuilder sb = new StringBuilder();
    foreach (var y in map)
    {
        foreach (var x in y.Value)
        {
            sb.Append(x.Value.ValueCh);
        }
    }
    return sb.ToString();
}