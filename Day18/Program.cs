using AOCHelper;
using System.ComponentModel.Design;
using System.Text;

var input = File.ReadAllLines("input.txt").ToList();

var l = new string(Enumerable.Range(0, 225).Select(_ => '.').ToArray());
var map = Helper.ParseInput(Enumerable.Range(0, 225).Select(_ => l).ToList());

List<Line> oldLines = new List<Line>();
List<Line> lines = new List<Line>();
var occ = new Coordinate { X = 0, Y = 0 };
var cc = new Coordinate { X = 0, Y = 0 };

foreach (var line in input)
{
    var sr = line.Split(" ");
    var hh = sr[2].Trim('(', ')').Replace("#", "0x");
    var inst = int.Parse(hh.Last().ToString());
    var len = Convert.ToInt32(new string(hh.SkipLast(1).ToArray()), 16);
    var olen = int.Parse(sr[1]);

    Coordinate nc = null;
    Coordinate onc = null;

    switch (sr[0])
    {
        case "R":
            onc = new Coordinate { X = occ.X + olen, Y = occ.Y };
            oldLines.Add(new Line { Start = occ, End = onc });
            break;
        case "L":
            onc = new Coordinate { X = occ.X - olen, Y = occ.Y };
            oldLines.Add(new Line { Start = onc, End = occ });
            break;
        case "D":
            onc = new Coordinate { X = occ.X, Y = occ.Y + olen };
            oldLines.Add(new Line { Start = occ, End = onc });
            break;
        case "U":
            onc = new Coordinate { X = occ.X, Y = occ.Y - olen };
            oldLines.Add(new Line { Start = onc, End = occ });
            break;
    }

    switch (inst)
    {
        case 0:
            nc = new Coordinate { X = cc.X + len, Y = cc.Y };
            lines.Add(new Line { Start = cc, End = nc });
            break;
        case 1:
            nc = new Coordinate { X = cc.X, Y = cc.Y + len };
            lines.Add(new Line { Start = cc, End = nc });
            break;
        case 2:
            nc = new Coordinate { X = cc.X - len, Y = cc.Y };
            lines.Add(new Line { Start = nc, End = cc });
            break;
        case 3:
            nc = new Coordinate { X = cc.X, Y = cc.Y - len };
            lines.Add(new Line { Start = nc, End = cc });
            break;
    }

    cc = nc;
    occ = onc;
}

var hLines = lines.Where(l => l.Start.Y == l.End.Y).OrderBy(l => l.Start.Y).ToList();
var vLines = lines.Where(l => l.Start.X == l.End.X).OrderBy(l => l.Start.X).ToList();

decimal ret2 = 0;
long currentPosY = hLines.Min(l => l.Start.Y);
long maxY = hLines.Max(l => l.Start.Y);

while(currentPosY < maxY)
{
    var lc = CalculateLineValue(currentPosY, vLines, hLines);
    var ncp = hLines.Where(l => l.Start.Y > currentPosY).Min(l => l.Start.Y);
    ret2 += lc * (ncp - currentPosY);

    bool doNextLine = true;
    while (doNextLine)
    {
        var nc = CalculateLineValue(ncp, vLines, hLines);
        ret2 += nc;
        ncp++;
        doNextLine = hLines.Any(hl => hl.Start.Y == ncp);
    }

    currentPosY = ncp;
}

Console.ReadKey();

long CalculateLineValue(long y, List<Line> vLines, List<Line> hLines)
{
    long ret = 0;
    var crosses = vLines.Where(l => l.Start.Y <= y && l.End.Y >= y).ToList();
    var horizontalCrosses = hLines.Where(l => l.Start.Y == y).ToList();

    HashSet<long> visitedXs = new HashSet<long>();
    bool isIn = false;
    var cx = crosses[0].Start.X;
    var lastx = crosses.Last().Start.X;

    while(cx <= lastx)
    {
        var currV = crosses.Single(vl => vl.Start.X == cx);
        ret++;
        cx++;
        var hc = horizontalCrosses.SingleOrDefault(hl => hl.Start.X <= cx && hl.End.X >= cx);
        var nextV = crosses.SkipWhile(vl => vl.End.X < cx).FirstOrDefault();
        if(hc == null)
        {
            isIn = !isIn;
            if(nextV != null)
            {
                var nextx = nextV.Start.X;
                if (isIn)
                {
                    ret += nextx - cx;
                }
                cx = nextx;
            }
            else
            {
                break;
            }
        }
        else
        {
            ret += hc.End.X - hc.Start.X;
            cx = hc.End.X + 1;

            if((currV.Start.Y == y && nextV.End.Y == y)
                || (currV.End.Y == y && nextV.Start.Y == y))
            {
                isIn = !isIn;
            }

            var nextnextV = crosses.SkipWhile(vl => vl.End.X < cx).FirstOrDefault();
            if (nextnextV != null)
            {
                var nextx = nextnextV.Start.X;
                if (isIn)
                {
                    ret += nextx - cx;
                }
                cx = nextx;
            }
        }
    }
    
    return ret;
}

class Line
{
    public Coordinate Start { get; set; }
    public Coordinate End { get; set; }
}