using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    public class Item
    {
        public char ValueCh { get; set; }
        public string ValueStr { get; set; }
        
        public Item Clone()
        {
            return new Item { ValueCh = ValueCh, ValueStr = ValueStr, GearNum = GearNum, GearValue = GearValue };
        }

        public int GearNum { get; set; } = 0;
        public long GearValue { get; set; } = 1;
    }

    public class Helper
    {
        public static Dictionary<int, Dictionary<int, Item>> ParseInput(List<string> input)
        {
            var ret = new Dictionary<int, Dictionary<int, Item>>();
            var c = input[0].Length;
            for (int i = 0; i < input.Count; i++)
            {
                ret.Add(i, new Dictionary<int, Item>());
                for (int j = 0; j < c; j++)
                {
                    ret[i].Add(j, new Item { ValueCh = input[i][j], ValueStr = input[i][j].ToString() });
                }
            }

            return ret;
        }

        public static Dictionary<int, Dictionary<int, Item>> ParseInputWithPadding(List<string> input, Item padItem)
        {
            var ret = ParseInput(input);
            var c = input[0].Length;
            ret.Add(-1, new Dictionary<int, Item>());
            ret.Add(input.Count, new Dictionary<int, Item>());
            for(int i = -1; i < c + 1; i++)
            {
                ret[-1].Add(i, padItem.Clone());
                ret[input.Count].Add(i, padItem.Clone());
            }

            for(int i=0; i < input.Count; i++)
            {
                ret[i].Add(-1, padItem.Clone());
                ret[i].Add(c, padItem.Clone());
            }

            return ret;
        }

        public static List<Item> Adjacent(Dictionary<int, Dictionary<int, Item>> items, int line, int pos)
        {
            List<Item> ret =
            [
                items[line - 1][pos],
                items[line][pos - 1],
                items[line][pos + 1],
                items[line + 1][pos],
            ];

            return ret;
        }

        public static List<Item> AdjacentAndDiag(Dictionary<int, Dictionary<int, Item>> items, int line, int pos)
        {
            List<Item> ret =
            [
                items[line - 1][pos - 1],
                items[line - 1][pos],
                items[line - 1][pos + 1],
                items[line][pos - 1],
                items[line][pos + 1],
                items[line + 1][pos - 1],
                items[line + 1][pos],
                items[line + 1][pos + 1],
            ];

            return ret;
        }
    }
}
