var input = File.ReadAllLines("input.txt").ToList();

List<Brick> allBricks = new List<Brick>();
int ln = 0;
foreach(var line in input)
{
    var sp = line.Split("~");
    var c1s = sp[0].Split(",").Select(s=>int.Parse(s)).ToList();
    var c2s = sp[1].Split(",").Select(s => int.Parse(s)).ToList();
    Brick b = new Brick { Id = ln };
    if (c1s[0] == c2s[0])
        b.X = c1s[0];
    else
    {
        b.X = Math.Min(c1s[0], c2s[0]);
        var diff = Math.Abs(c1s[0] - c2s[0]) + 1;
        b.Xs = Enumerable.Range(b.X, diff).ToHashSet();
    }

    if (c1s[1] == c2s[1])
        b.Y = c1s[1];
    else
    {
        b.Y = Math.Min(c1s[1], c2s[1]);
        var diff = Math.Abs(c1s[1] - c2s[1]) + 1;
        b.Ys = Enumerable.Range(b.Y, diff).ToHashSet();
    }

    if (c1s[2] == c2s[2])
        b.Z = c1s[2];
    else
    {
        b.Z = Math.Min(c1s[2], c2s[2]);
        var diff = Math.Abs(c1s[2] - c2s[2]) + 1;
        b.Zs = Enumerable.Range(b.Z, diff).ToHashSet();
    }

    if(!b.Xs.Any() && !b.Ys.Any() && !b.Zs.Any())
    {
        b.Zs.Add(b.Z);
    }

    allBricks.Add(b);
    ln++;
}


List<Brick> staticBricks = allBricks.Where(b=>b.Z == 1).ToList();
var fallingBricks = allBricks.Except(staticBricks).OrderBy(b => b.Z).ToList();

while (fallingBricks.Count > 0)
{
    var lowestBrick = fallingBricks.First();
    var canFall = lowestBrick.CanFall(staticBricks);
    while(canFall)
    {
        lowestBrick.Fall();
        canFall = lowestBrick.CanFall(staticBricks);
    }

    staticBricks.Add(lowestBrick);
    fallingBricks = fallingBricks.Skip(1).ToList();
}

var ret1 = allBricks.Count(b => b.CanDisintegrate());
var ret2 = allBricks.Sum(b => b.AllSupportedBrickCount(new List<Brick> { b }).Count);

Console.ReadLine();

class Brick
{
    public int Id { get; set; }
    public HashSet<int> Xs { get; set; } = new HashSet<int>();
    public HashSet<int> Ys { get; set; } = new HashSet<int>();
    public HashSet<int> Zs { get; set; } = new HashSet<int>();

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public List<Brick> SupportedBy { get; set; } = new List<Brick>();
    public List<Brick> SupportedBricks { get; set; } = new List<Brick>();

    public bool CanDisintegrate()
    {
        if(!SupportedBricks.Any())
        {
            return true;
        }

        return SupportedBricks.All(sb => sb.SupportedBy.Count > 1);
    }

    public List<Brick> AllSupportedBrickCount(List<Brick> removedBricks)
    {   
        var fbs = SupportedBricks.Where(sb => sb.SupportedBy.All(sbb=>removedBricks.Contains(sbb))).ToList();
        var ret = fbs.ToList();
        removedBricks.AddRange(fbs);
        foreach(var fb in fbs)
        {
            var sr = fb.AllSupportedBrickCount(removedBricks);
            ret.AddRange(sr);
        }

        ret = ret.Distinct().ToList();
        return ret;
    }

    public void Fall()
    {
        if (Zs.Any())
        {
            Zs = Zs.Select(z => z - 1).ToHashSet();
        }
        Z--;
    }

    public bool CanFall(List<Brick> otherBricks)
    {
        if (Z == 1)
            return false;

        var supporters = otherBricks.Where(b => IsSupported(b)).ToList();
        if(!supporters.Any())
        {
            return true;
        }

        SupportedBy = supporters;
        foreach (var s in supporters)
        {
            s.SupportedBricks.Add(this);
        }
        return false;
    }

    public bool IsSupported(int x, int y, int z)
    {
        if (Xs.Any() && Xs.Contains(x))
        {
            return Y == y && Z - 1 == z;
        }
        else if (Ys.Any() && Ys.Contains(y))
        {
            return X == x && Z - 1== z;
        }
        else if (Zs.Any() && Zs.Contains(z + 1))
        {
            return X == x && Y == y;
        }
        else
        {
            return false;
        }
    }

    public bool IsSupported(Brick b)
    {
        if (b.Xs.Any())
        {
            return b.Xs.Any(x => IsSupported(x, b.Y, b.Z));
        }
        else if (b.Ys.Any())
        {
            return b.Ys.Any(y => IsSupported(b.X, y, b.Z));
        }
        else if (b.Zs.Any())
        {
            return b.Zs.Any(z => IsSupported(b.X, b.Y, z));
        }
        else
        {
            throw new Exception();
        }
    }

    public bool IsIntersect(int x, int y, int z)
    {
        if(Xs.Any() && Xs.Contains(x))
        {
            return Y == y && Z == z;
        }
        else if (Ys.Any() && Ys.Contains(y))
        {
            return X == x && Z == z;
        }
        else if (Zs.Any() && Zs.Contains(z))
        {
            return X == x && Y == y;
        }
        else
        {
            throw new Exception();
        }
    }

    public bool IsIntersect(Brick b)
    {
        if(b.Xs.Any())
        {
            return b.Xs.Any(x => IsIntersect(x, b.Y, b.Z));
        }
        else if (b.Ys.Any())
        {
            return b.Ys.Any(y => IsIntersect(b.X, y, b.Z));
        }
        else if (b.Zs.Any())
        {
            return b.Zs.Any(z => IsIntersect(b.X, b.Y, z));
        }
        else
        {
            throw new Exception();
        }
    }

}