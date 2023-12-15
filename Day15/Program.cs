using System.Xml.Schema;

var input = File.ReadAllText("input.txt");

long ret1 = 0;

var cmds = input.Split(',');
foreach(var cmd in cmds)
{
    ret1 += Hash(cmd);
}

Dictionary<long, List<Item>> boxes = new Dictionary<long, List<Item>>();

foreach (var cmd in cmds)
{
    var label = new string(cmd.TakeWhile(c => char.IsLetter(c)).ToArray());
    var c = cmd.Substring(label.Length,1);
    var fl = cmd.Substring(label.Length + 1);

    var h = Hash(label);
    if(c == "-")
    {
        if(boxes.ContainsKey(h))
        {
            var item = boxes[h].SingleOrDefault(i => i.Label == label);
            if(item != null)
            {
                boxes[h].Remove(item);
            }
        }
    }
    else if (c == "=")
    {
        if(!boxes.ContainsKey(h))
        {
            boxes.Add(h, new List<Item>());
        }
        var item = boxes[h].SingleOrDefault(i => i.Label == label);
        if(item == null)
        {
            boxes[h].Add(new Item { Label = label, Value = int.Parse(fl) });
        }
        else
        {
            item.Value = int.Parse(fl);
        }
    }
}

long ret2 = 0;
foreach(var kvp in boxes)
{
    int i = 1;
    foreach(var item in kvp.Value)
    {
        ret2 += (kvp.Key + 1) * i * item.Value;
        i++;
    }
}

Console.ReadLine();


long Hash(string s)
{
    long ret = 0;
    foreach(var ch in s)
    {
        ret += (int)ch;
        ret *= 17;
        ret %= 256;
    }
    return ret;
}

class Item
{
    public string Label { get; set; }
    public int Value { get; set; }
}