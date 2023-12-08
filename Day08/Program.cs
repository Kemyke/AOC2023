using MathNet.Numerics;
using System.ComponentModel.Design;

var input = File.ReadAllLines("input.txt");
var steps = input.First();

Dictionary<string, (string, string)> network = new Dictionary<string, (string, string)>();
foreach (var inst in input.Skip(2))
{
    var sp = inst.Split(" = ");
    var cn = sp[0];
    var sp2 = sp[1].Split(", ");
    var left = sp2[0].Trim('(');
    var right = sp2[1].Trim(')');

    network.Add(cn, (left, right));
}

long ret1 = 0;
long stepNum = 0;
var currentNode = "AAA";

List<string> currentNodes = network.Keys.Where(k => k[2] == 'A').ToList();
Dictionary<int, long> firstZs = new Dictionary<int, long>();

for(int i = 0; i < int.MaxValue; i++)
{
    if (steps[i % steps.Length] == 'L')
    {
        //currentNode = network[currentNode].Item1;
        currentNodes = currentNodes.Select(cn => network[cn].Item1).ToList();
    }
    else
    {
        //currentNode = network[currentNode].Item2;
        currentNodes = currentNodes.Select(cn => network[cn].Item2).ToList();
    }
    //ret1++;
    stepNum++;
    //if(currentNode == "ZZZ")
    //{
    //    break;
    //}

    foreach(var cn in currentNodes.Where(c => c[2] == 'Z'))
    {
        var idx = currentNodes.IndexOf(cn);
        if(!firstZs.ContainsKey(idx))
        {
            firstZs.Add(idx, stepNum);
        }
    }

    if (firstZs.Count == 6)
        break;
}
var ret2 = Euclid.LeastCommonMultiple(firstZs.Values.ToArray());
Console.ReadLine();