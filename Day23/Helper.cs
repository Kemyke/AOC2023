namespace AOCHelper
{
    public class Coordinate
    {
        public long X { get; set; }
        public long Y { get; set; }

        public List<(string, Coordinate)> Adjacent()
        {
            List<(string, Coordinate)> ret =
            [
                ("N", new Coordinate { Y = Y - 1, X = X }),
                ("W", new Coordinate { Y = Y, X = X - 1 }),
                ("E", new Coordinate { Y = Y, X = X + 1 }),
                ("S", new Coordinate { Y = Y + 1, X = X }),
            ];

            return ret;
        }

        public List<Coordinate> AdjacentAndDiag()
        {
            List<Coordinate> ret =
            [
                new Coordinate { Y = Y - 1, X = X },
                new Coordinate { Y = Y, X = X - 1 },
                new Coordinate { Y = Y, X = X + 1 },
                new Coordinate { Y = Y + 1, X = X },

                new Coordinate { Y = Y - 1, X = X - 1 },
                new Coordinate { Y = Y - 1, X = X + 1 },
                new Coordinate { Y = Y + 1, X = X - 1 },
                new Coordinate { Y = Y + 1, X = X + 1 },

            ];

            return ret;
        }
    }
    public class Item
    {
        public string Id { get; set; }
        public uint DijkstraId { get; set; }
        public Coordinate Coordinate { get; set; }
        public char ValueCh { get; set; }
        //public string ValueStr { get; set; }
        public Dictionary<string, int> MaxValuesByReachable { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> Visiteds { get; set; } = new Dictionary<string, int>();

        public Item Clone()
        {
            return new Item { Coordinate = new Coordinate { X = Coordinate.X, Y = Coordinate.Y }, ValueCh = ValueCh};
        }
    }

    public class Helper
    {
        public static List<string> RotateStringList(List<string> input)
        {
            var rowLength = input[0].Length;
            if(!input.All(l=>l.Length == rowLength))
            {
                throw new Exception();
            }

            var ret = new List<string>(rowLength);
            for(var i = 0; i < rowLength; i++)
            {
                ret.Add(new string(input.Select(l => l[i]).ToArray()));
            }

            return ret;
        }

        public static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input)
        {
            var ret = new Dictionary<long, Dictionary<long, Item>>();
            var c = input[0].Length;
            for (int i = 0; i < input.Count; i++)
            {
                ret.Add(i, new Dictionary<long, Item>());
                for (int j = 0; j < c; j++)
                {
                    var item = new Item { Id = (1000 * i + j).ToString(), Coordinate = new Coordinate { X = j, Y = i }, ValueCh = input[i][j] };
                    ret[i].Add(j, item);
                }
            }

            return ret;
        }

        //#region Dijkstra

        //public static Dijkstra.NET.Graph.Graph<Item, long> BuildGraph(Dictionary<long, Dictionary<long, Item>> map)
        //{
        //    Dijkstra.NET.Graph.Graph<Item, long> ret = new Dijkstra.NET.Graph.Graph<Item, long>();
        //    foreach(var item in map.SelectMany(kvp=>kvp.Value.Values))
        //    {
        //        var idi = ret.AddNode(item);
        //        item.DijkstraId = idi;
        //    }

        //    return ret;
        //}

        //public static void ConnectAdjacent(Dictionary<long, Dictionary<long, Item>>  map, Dijkstra.NET.Graph.Graph<Item, long> graph)
        //{
        //    foreach(var item in map.SelectMany(kvp=>kvp.Value.Values))
        //    {
        //        foreach(var ai in AdjacentItems(map, item))
        //        {
        //            graph.Connect(item.DijkstraId, ai.DijkstraId, 1, 0);
        //            graph.Connect(ai.DijkstraId, item.DijkstraId, 1, 0);
        //        }
        //    }
        //}

        //public static void ConnectAdjacentAndDiag(Dictionary<long, Dictionary<long, Item>> map, Dijkstra.NET.Graph.Graph<Item, long> graph)
        //{
        //    foreach (var item in map.SelectMany(kvp => kvp.Value.Values))
        //    {
        //        foreach (var ai in AdjacentAndDiagItems(map, item))
        //        {
        //            graph.Connect(item.DijkstraId, ai.DijkstraId, 1, 0);
        //            graph.Connect(ai.DijkstraId, item.DijkstraId, 1, 0);
        //        }
        //    }
        //}

        //#endregion

        public static List<string> PadInput(List<string> input, char padCh)
        {
            var ret = input.ToList();
            var c = input[0].Length;
            for (int i = 0; i < c; i++)
            {
                ret[i] = padCh.ToString() + input[i] + padCh.ToString();
                
            }
            ret.Insert(0, new string(Enumerable.Range(0, c + 2).Select(_ => padCh).ToArray()));
            ret.Add(new string(Enumerable.Range(0, c + 2).Select(_ => padCh).ToArray()));
            return ret;
        }

        //public static List<Item> AdjacentItems(Dictionary<long, Dictionary<long, Item>> map, int y, int x)
        //{
        //    return AdjacentItems(map, map[y][x]);
        //}

        //public static List<Item> AdjacentItems(Dictionary<long, Dictionary<long, Item>> map, Item item)
        //{
        //    List<Item> ret = new List<Item>();
        //    foreach (var c in item.Coordinate.Adjacent())
        //    {
        //        if (map.ContainsKey(c.Y) && map[c.Y].ContainsKey(c.X))
        //            ret.Add(map[c.Y][c.X]);
        //    }
        //    return ret;
        //}

        public static List<(string, Item)> AdjacentPossiblePlaces(Dictionary<long, Dictionary<long, Item>> map, Item item, List<Item> steps, Item lastItem)
        {
            var ret = new List<(string, Item)>();
            foreach (var c in item.Coordinate.Adjacent())
            {
                if (map.ContainsKey(c.Item2.Y) && map[c.Item2.Y].ContainsKey(c.Item2.X))
                {
                    var ci = map[c.Item2.Y][c.Item2.X];

                    if (lastItem == ci || steps.Contains(ci))
                    {
                        continue;
                    }

                    if (ci.ValueCh == '#')
                    {
                        continue;
                    }
                    ret.Add((c.Item1, ci));

                    //if (ci.ValueCh == '.')
                    //{
                    //    ret.Add((c.Item1, ci));
                    //}
                    //else if(ci.ValueCh == '>' && c.Item1 == "E")
                    //{
                    //    ret.Add((c.Item1, ci));
                    //}
                    //else if (ci.ValueCh == 'v' && c.Item1 == "S")
                    //{
                    //    ret.Add((c.Item1, ci));
                    //}
                    //else if (ci.ValueCh == '<' && c.Item1 == "W")
                    //{
                    //    ret.Add((c.Item1, ci));
                    //}
                    //else if (ci.ValueCh == '^' && c.Item1 == "N")
                    //{
                    //    ret.Add((c.Item1, ci));
                    //}
                }
            }
            return ret;
        }

        public static List<Item> AdjacentAndDiagItems(Dictionary<long, Dictionary<long, Item>> map, int y, int x)
        {
            return AdjacentAndDiagItems(map, map[y][x]);
        }

        public static List<Item> AdjacentAndDiagItems(Dictionary<long, Dictionary<long, Item>> map, Item item)
        {
            List<Item> ret = new List<Item>();
            foreach (var c in item.Coordinate.AdjacentAndDiag())
            {
                if (map.ContainsKey(c.Y) && map[c.Y].ContainsKey(c.X))
                    ret.Add(map[c.Y][c.X]);
            }
            return ret;
        }

        public static void VisualizeStringList(List<string> input)
        {
            foreach (var l in input)
            {
                Console.WriteLine(l);
            }
            Console.WriteLine();
        }

        public static void VisualizeMap(Dictionary<long, Dictionary<long, Item>> map)
        {
            foreach (var y in map)
            {
                foreach (var x in y.Value)
                {
                    Console.Write(x.Value.ValueCh);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
