using System.Collections.Generic;
using System.Numerics;

var input = File.ReadAllLines("input.txt").ToList();

Dictionary<string, Workflow> Workflows = new Dictionary<string, Workflow>();
List<Dictionary<string, long>> accepteds = new List<Dictionary<string, long>>();

var ws = input.TakeWhile(l => l != "").ToList();
var vs = input.Skip(ws.Count + 1).ToList();

foreach(var w in ws)
{
    var name = new string(w.TakeWhile(c => c != '{').ToArray());
    var rulesStr = w.Replace(name + "{", "").Trim('}').Split(",").ToList();
    var lastRule = new Rule { NextWorkflow = rulesStr.Last() };
    var normalRules = rulesStr.SkipLast(1).ToList();
    List<Rule> rules = new List<Rule>();
    foreach(var nr in normalRules)
    {
        var ps = nr.Split(":");
        var nxr = ps[1];
        if (ps[0].Contains('>'))
        {
            var r = ps[0].Split(">");
            var rule = new Rule { Variable = r[0], Relation = ">", Value = long.Parse(r[1]), NextWorkflow = nxr };
            rules.Add(rule);
        }
        else
        {
            var r = ps[0].Split("<");
            var rule = new Rule { Variable = r[0], Relation = "<", Value = long.Parse(r[1]), NextWorkflow = nxr };
            rules.Add(rule);
        }
    }
    rules.Add(lastRule);
    Workflows.Add(name, new Workflow { Rules = rules });
}

var inw = Workflows["in"];
foreach (var v in vs)
{
    Dictionary<string, long> vars = new Dictionary<string, long>();
    var sls = v.Trim('{', '}').Split(",").Select(r => (r.Split("=")[0], long.Parse(r.Split("=")[1]))).ToList();
    foreach (var sl in sls)
        vars.Add(sl.Item1, sl.Item2);

    var r = inw.Evalulate(Workflows, vars);
    if (r)
    {
        accepteds.Add(vars);
    }
}

var ret1 = accepteds.Sum(kvp => kvp.Values.Sum());

var iis = new Dictionary<string, Interval>();
iis.Add("x", new Interval { Low = 1, High = 4000 });
iis.Add("m", new Interval { Low = 1, High = 4000 });
iis.Add("a", new Interval { Low = 1, High = 4000 });
iis.Add("s", new Interval { Low = 1, High = 4000 });
var ret2List = new List<Dictionary<string, Interval>>();
inw.Evalulate2(Workflows, iis, ret2List);

BigInteger ret2 = 0;
foreach (var iv in ret2List)
{
    BigInteger sr = 1;
    foreach (var i in iv.Values)
        sr *= (i.High - i.Low + 1);
    ret2 += sr;
}

Console.ReadKey();





class Workflow
{
    public List<Rule> Rules { get; set; }

    public bool Evalulate(Dictionary<string, Workflow> workflows, Dictionary<string, long> values)
    {
        foreach(var rule in Rules)
        {
            var ret = rule.Evaluate(values);
            if(ret)
            {
                if (rule.NextWorkflow == "A")
                    return true;
                else if (rule.NextWorkflow == "R")
                    return false;
                else
                    return workflows[rule.NextWorkflow].Evalulate(workflows, values);
            }
        }

        throw new Exception();
    }

    public void Evalulate2(Dictionary<string, Workflow> workflows, Dictionary<string, Interval> intervals, List<Dictionary<string, Interval>> results)
    {
        var nis = intervals.ToDictionary(k => k.Key, v => v.Value.Clone());

        foreach (var rule in Rules)
        {

            if (rule.Variable == null)
            {
                if (rule.NextWorkflow == "A")
                {
                    results.Add(nis);
                    return;

                }
                else if (rule.NextWorkflow == "R")
                {
                    return;

                }
                else
                {
                    workflows[rule.NextWorkflow].Evalulate2(workflows, nis, results);
                }
            }
            else
            {
                var nis2 = nis.ToDictionary(k => k.Key, v => v.Value.Clone());
                var iv = nis2[rule.Variable];
                if (rule.Relation == "<")
                {
                    iv.High = rule.Value - 1;
                }
                else
                {
                    iv.Low = rule.Value + 1;
                }

                if (rule.NextWorkflow == "A")
                {
                    results.Add(nis2);
                }
                else if (rule.NextWorkflow == "R")
                {
                }
                else
                {
                    workflows[rule.NextWorkflow].Evalulate2(workflows, nis2, results);
                }
            }

            if (rule.Variable != null)
            {
                var iv = nis[rule.Variable];
                if (rule.Relation == "<")
                {
                    iv.Low = rule.Value;
                }
                else
                {
                    iv.High = rule.Value;
                }
            }
        }
    }
}

class Interval
{
    public long Low { get; set; }
    public long High { get; set; }

    public Interval Clone()
    {
        return new Interval { Low = Low, High = High };
    }

    public bool InInterval(long v)
    {
        return v >= Low && v <= High;
    }
}

class Rule
{
    public string Variable { get; set; }
    public string Relation { get; set; }
    public long Value { get; set; }
    public string NextWorkflow { get; set; }

    public bool Evaluate(Dictionary<string, long> values)
    {
        if (Variable == null)
            return true;

        var sv = values[Variable];
        switch(Relation)
        {
            case ">":
                return sv > Value;
            case "<": 
                return sv < Value;
        }
        throw new Exception();
    }
}