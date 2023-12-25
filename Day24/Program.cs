var input = File.ReadAllLines("input.txt").ToList();
List<HalfLine> lines = new List<HalfLine>();

var t = HalfLine.Create(new Coordinate { X = 3, Y = 4 }, 9, -6);

foreach(var line in input)
{
    var sp = line.Split(" @ ");
    var coords = sp[0].Split(", ").Select(s => double.Parse(s)).ToList();
    var velos = sp[1].Split(", ").Select(s => double.Parse(s)).ToList();
    var l = HalfLine.Create(new Coordinate { X = coords[0], Y = coords[1] }, velos[0], velos[1]);
    lines.Add(l);
}

long ret1 = 0;
double minArea = 200000000000000;
double maxArea = 400000000000000;

for (int i = 0; i < lines.Count - 1; i++)
{
    for (int j = i + 1; j < lines.Count; j++)
    {
        var l1 = lines[i];
        var l2 = lines[j];

        var intersection = HalfLine.Intersect(l1, l2);
        if (intersection != null)
        {

            if (intersection.X >= minArea && intersection.X <= maxArea && intersection.Y >= minArea && intersection.Y <= maxArea)
            {
                if (l1.IsLinePointOnHalfLine(intersection) && l2.IsLinePointOnHalfLine(intersection))
                {
                    ret1++;
                }
            }
        }
    }
}


Console.ReadKey();

class Coordinate
{
    public double X { get; set; }
    public double Y { get; set; }
}

class HalfLine
{
    public double M { get; set; }
    public double C { get; set; }

    public Coordinate StartPoint { get; set; }
    public double DX { get; set; }
    public double DY { get; set; }

    public static HalfLine Create(Coordinate coord, double dx, double dy)
    {
        var ret = new HalfLine();
        ret.StartPoint = coord;
        ret.DX = dx;
        ret.DY = dy;
        ret.M = dy / dx;
        ret.C = coord.Y - ret.M * coord.X;
        return ret;
    }

    public bool IsLinePointOnHalfLine(Coordinate point)
    {
        var xok = false;
        var yok = false;
        if (DX == 0)
            xok = true;
        else if (DX > 0 && point.X >= StartPoint.X)
            xok = true;
        else if (DX < 0 && point.X <= StartPoint.X)
            xok = true;

        if (DY == 0)
            yok = true;
        else if (DY > 0 && point.Y >= StartPoint.Y)
            yok = true;
        else if (DY < 0 && point.Y <= StartPoint.Y)
            yok = true;

        return xok && yok;
    }

    public static Coordinate Intersect(HalfLine l1, HalfLine l2)
    {
        var delta = l1.M - l2.M;
        if (delta == 0)
            return null;

        var ret = new Coordinate();

        ret.X = (l2.C - l1.C) / delta;
        ret.Y = (l1.M * l2.C - l2.M * l1.C) / delta;

        return ret;
    }
}

///kox + t0 * kovx == 200027938836082 + t0 * 133
///koy + t0 * kovy == 135313515251542 + t0 * 259
///koz + t0 * kovz == 37945458137479 + t0 * 506
///kox + t1 * kovx == 285259862606823 + t1 * 12
///koy + t1 * kovy == 407476720802151 + t1 * -120
///koz + t1 * kovz == 448972585175416  + t1 * -241
///kox + t2 * kovx == 329601664688534 + t2 * -133
///koy + t2 * kovy == 370686722303193 + t2 * -222
///koz + t2 * kovz == 178908568819244 + t2 * 168

