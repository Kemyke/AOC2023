using Dijkstra.NET.ShortestPath;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCHelper
{
    public class Coordinate
    {
        public long X { get; set; }
        public long Y { get; set; }

        public List<Coordinate> Adjacent()
        {
            List<Coordinate> ret =
            [
                new Coordinate { Y = Y - 1, X = X },
                new Coordinate { Y = Y, X = X - 1 },
                new Coordinate { Y = Y, X = X + 1 },
                new Coordinate { Y = Y + 1, X = X },
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
        public uint DijkstraId { get; set; }
        public Coordinate Coordinate { get; set; }
        public char ValueCh { get; set; }
        public string ValueStr { get; set; }

        public Item Clone()
        {
            return new Item 
            { 
                Coordinate = new Coordinate { X = Coordinate.X, Y = Coordinate.Y }, 
                ValueCh = ValueCh, ValueStr = ValueStr 
            };
        }
    }

    public class Helper
    {
        public static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input)
        {
            var ret = new Dictionary<long, Dictionary<long, Item>>();
            var c = input[0].Length;
            for (int i = 0; i < input.Count; i++)
            {
                ret.Add(i, new Dictionary<long, Item>());
                for (int j = 0; j < c; j++)
                {
                    var item = new Item { Coordinate = new Coordinate { X = j, Y = i }, ValueCh = input[i][j], ValueStr = input[i][j].ToString() };
                    ret[i].Add(j, item);
                }
            }

            return ret;
        }

        #region Dijkstra

        public static Dijkstra.NET.Graph.Graph<Item, long> BuildGraph(Dictionary<long, Dictionary<long, Item>> map)
        {
            Dijkstra.NET.Graph.Graph<Item, long> ret = new Dijkstra.NET.Graph.Graph<Item, long>();
            foreach(var item in map.SelectMany(kvp=>kvp.Value.Values))
            {
                var idi = ret.AddNode(item);
                item.DijkstraId = idi;
            }

            return ret;
        }

        public static void ConnectAdjacent(Dictionary<long, Dictionary<long, Item>>  map, Dijkstra.NET.Graph.Graph<Item, long> graph)
        {
            foreach(var item in map.SelectMany(kvp=>kvp.Value.Values))
            {
                foreach(var ai in AdjacentItems(map, item))
                {
                    graph.Connect(item.DijkstraId, ai.DijkstraId, 1, 0);
                    graph.Connect(ai.DijkstraId, item.DijkstraId, 1, 0);
                }
            }
        }

        public static void ConnectAdjacentAndDiag(Dictionary<long, Dictionary<long, Item>> map, Dijkstra.NET.Graph.Graph<Item, long> graph)
        {
            foreach (var item in map.SelectMany(kvp => kvp.Value.Values))
            {
                foreach (var ai in AdjacentAndDiagItems(map, item))
                {
                    graph.Connect(item.DijkstraId, ai.DijkstraId, 1, 0);
                    graph.Connect(ai.DijkstraId, item.DijkstraId, 1, 0);
                }
            }
        }

        #endregion

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

        public static List<Item> AdjacentItems(Dictionary<long, Dictionary<long, Item>> map, int y, int x)
        {
            return AdjacentItems(map, map[y][x]);
        }

        public static List<Item> AdjacentItems(Dictionary<long, Dictionary<long, Item>> map, Item item)
        {
            List<Item> ret = new List<Item>();
            foreach (var c in item.Coordinate.Adjacent())
            {
                if (map.ContainsKey(c.Y) && map[c.Y].ContainsKey(c.X))
                    ret.Add(map[c.Y][c.X]);
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
    }
}
