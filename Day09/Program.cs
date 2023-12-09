using System.Net.WebSockets;

var input = File.ReadAllLines("input.txt");

long ret2 = 0;

foreach(var line in input)
{
    var nums = line.Split(" ").Select(s=>long.Parse(s)).ToList();
    var cns = new List<List<long>> { nums };
    var cn = nums.ToList();
    while(!cn.All(n=>n==0))
    {
        var nl = new List<long>();
        for(int i = 1; i<cn.Count;i++)
        {
            nl.Add(cn[i] - cn[i - 1]);
        }

        cns.Add(nl);
        cn = nl;
    }

    long lastDiff = 0;
    for(var i = cns.Count - 1; i >= 0; i--)
    {
        cns[i].Insert(0, cns[i].First() - lastDiff);
        lastDiff = cns[i].First();
    }
    var x = cns[0].First();
    ret2 += x;
}

Console.ReadLine();