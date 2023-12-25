var input = File.ReadAllLines("input.txt").ToList();

var components = new Dictionary<string, Component>();

foreach (var line in input)
{
    var names = line.Replace(":", "").Split(" ");
    foreach(var name in names)
    {
        if(!components.ContainsKey(name))
        {
            components.Add(name, new Component { Name = name });
        }
    }
}

foreach (var line in input)
{
    var sr = line.Split(": ");
    var c = components[sr[0]];
    foreach (var cc in sr[1].Split(" "))
    {
        var ccomp = components[cc];
        if (!c.ConnectionNums.ContainsKey(cc))
        {
            c.ConnectionNums.Add(cc, 1);
        }
        
        if (!ccomp.ConnectionNums.ContainsKey(c.Name))
        {
            ccomp.ConnectionNums.Add(c.Name, 1);
        }
    }
}

int ret1;
while(true)
{
    var rand = new Random();
    var contractedComponents = components.ToDictionary(k => k.Key, v => v.Value.Clone());
    while (contractedComponents.Count > 2)
    {
        var i1 = rand.Next(contractedComponents.Count);
        var c1 = contractedComponents.Values.Skip(i1).First();
        var c2 = contractedComponents[c1.ConnectionNums.OrderByDescending(kvp => kvp.Value).First().Key];
        ContractNodes(contractedComponents, c1, c2);
    }

    var mc = contractedComponents.Values.First().ConnectionNums.Values.Single();
    if (mc == 3)
    {
        ret1 = contractedComponents.First().Value.Size * contractedComponents.Skip(1).First().Value.Size;
        break;
    }
}

Console.ReadLine();

void ContractNodes(Dictionary<string, Component> contcomponents, Component c1, Component c2)
{
    c1.Size += c2.Size;
    contcomponents.Remove(c2.Name);
    c1.ConnectionNums.Remove(c2.Name);
    c2.ConnectionNums.Remove(c1.Name);

    foreach (var cc in c2.ConnectionNums)
    {
        if (!c1.ConnectionNums.ContainsKey(cc.Key))
        {
            c1.ConnectionNums.Add(cc.Key, c2.ConnectionNums[cc.Key]);
        }
        else
        {
            c1.ConnectionNums[cc.Key] += c2.ConnectionNums[cc.Key];
        }

        var ccc = contcomponents[cc.Key];
        if (!ccc.ConnectionNums.ContainsKey(c1.Name))
        {
            ccc.ConnectionNums.Add(c1.Name, cc.Value);
        }
        else
        {
            ccc.ConnectionNums[c1.Name] += cc.Value;
        }
        ccc.ConnectionNums.Remove(c2.Name);
    }
}

class Component
{
    public string Name { get; set; }
    public Dictionary<string, int> ConnectionNums { get; set; } = new Dictionary<string, int> { };
    public int Size { get; set; } = 1;

    public Component Clone()
    {
        return new Component { Name = Name, ConnectionNums = ConnectionNums.ToDictionary(k=>k.Key, v => v.Value) };
    }
}