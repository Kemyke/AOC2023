using AOCHelper;
using System.Net.Http.Headers;

var input = File.ReadAllLines("input.txt").ToList();

var ret2 = 0;
for (int i = 0; i < input[0].Length; i++)
{
    var map = Helper.ParseInput(input);
    List<Beam> beams = new List<Beam>();
    beams.Add(new Beam { CurrentPos = new Item { Coordinate = new Coordinate { X = -1, Y = i } }, LastDir = "E", PathStr = "" });

    while (beams.Any())
    {
        var beam = beams.First();
        var nextPos = new Coordinate { X = beam.CurrentPos.Coordinate.X, Y = beam.CurrentPos.Coordinate.Y };
        switch (beam.LastDir)
        {
            case "E":
                nextPos.X = nextPos.X + 1;
                break;
            case "W":
                nextPos.X = nextPos.X - 1;
                break;
            case "N":
                nextPos.Y = nextPos.Y - 1;
                break;
            case "S":
                nextPos.Y = nextPos.Y + 1;
                break;
        }

        if (map.ContainsKey(nextPos.Y) && map[nextPos.Y].ContainsKey(nextPos.X))
        {
            var ci = map[nextPos.Y][nextPos.X];

            if (ci.Lights.ContainsKey(beam.LastDir))
            {
                beams = beams.Skip(1).ToList();
                continue;
            }

            ci.Energized = true;
            ci.Lights[beam.LastDir] = 1;

            if (ci.ValueCh != '.')
            {
                switch (beam.LastDir)
                {
                    case "E":
                        switch (ci.ValueCh)
                        {
                            case '/':
                                beam.LastDir = "N";
                                break;
                            case '\\':
                                beam.LastDir = "S";
                                break;
                            case '|':
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "N" });
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "S" });
                                beams = beams.Skip(1).ToList();

                                break;
                        }
                        break;
                    case "W":
                        switch (ci.ValueCh)
                        {
                            case '/':
                                beam.LastDir = "S";
                                break;
                            case '\\':
                                beam.LastDir = "N";
                                break;
                            case '|':
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "N" });
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "S" });
                                beams = beams.Skip(1).ToList();

                                break;
                        }
                        break;
                    case "N":
                        switch (ci.ValueCh)
                        {
                            case '/':
                                beam.LastDir = "E";
                                break;
                            case '\\':
                                beam.LastDir = "W";
                                break;
                            case '-':
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "W" });
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "E" });
                                beams = beams.Skip(1).ToList();
                                break;
                        }
                        break;
                    case "S":
                        switch (ci.ValueCh)
                        {
                            case '/':
                                beam.LastDir = "W";
                                break;
                            case '\\':
                                beam.LastDir = "E";
                                break;
                            case '-':
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "W" });
                                beams.Add(new Beam { CurrentPos = ci, LastDir = "E" });
                                beams = beams.Skip(1).ToList();
                                break;
                        }
                        break;
                }
            }
            beam.CurrentPos = ci;
        }
        else
        {
            beams = beams.Skip(1).ToList();
        }
    }
    var sr = map.SelectMany(k => k.Value).Where(k => k.Value.Energized).Count();
    if (sr > ret2)
        ret2 = sr;
    //var ret1 = map.SelectMany(k => k.Value).Where(k => k.Value.Energized).Count();
}
Console.ReadLine();
