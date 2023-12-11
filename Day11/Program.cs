using AOCHelper;
using Dijkstra.NET.ShortestPath;

var input = File.ReadAllLines("input.txt").ToList();
List<string> expInput = new List<string>();
List<int> emptyRows = new List<int>();
for(int i = 0; i < input.Count; i++)
{
    expInput.Add(input[i]);
    if (input[i].All(c=>c=='.'))
    {
        emptyRows.Add(i);
    }
}

List<int> emptyColumns = new List<int>();
for (int i = 0; i < expInput[0].Length; i++)
{
    if (expInput.All(l => l[i] == '.'))
    {
        emptyColumns.Add(i);
    }
}

var map = Helper.ParseInput(input);
long ret1 = 0;
long ret2 = 0;

var galaxies = map.SelectMany(kvp => kvp.Value.Values).Where(i => i.ValueCh == '#').ToList();
for(int i = 0; i < galaxies.Count - 1; i++)
{
    var g1 = galaxies[i];


    for (int j = i + 1; j < galaxies.Count; j++)
    {
        var g2 = galaxies[j];

        var diff = Math.Abs(g1.Coordinate.X - g2.Coordinate.X) + Math.Abs(g1.Coordinate.Y - g2.Coordinate.Y);
        ret1 += diff;
        ret2 += diff;
        var miny = Math.Min(g1.Coordinate.Y, g2.Coordinate.Y);
        var maxy = Math.Max(g1.Coordinate.Y, g2.Coordinate.Y);
        var rc = emptyRows.Count(r => r > miny && r < maxy);
        ret1 += rc;
        ret2 += 999999 * rc; 

        var minx = Math.Min(g1.Coordinate.X, g2.Coordinate.X);
        var maxx = Math.Max(g1.Coordinate.X, g2.Coordinate.X);
        var cc = emptyColumns.Count(r => r > minx && r < maxx);
        ret1 += cc;
        ret2 += 999999 * cc;

    }
}

Console.ReadLine();