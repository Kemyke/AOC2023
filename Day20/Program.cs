using System.Diagnostics;

var input = File.ReadAllLines("input.txt").ToList();
foreach(var line in input)
{
    var sp = line.Split(" -> ");

    IModule m;
    if (sp[0] == "broadcaster")
        m = new BroadcasterModule { Name = sp[0] };
    else if (sp[0].StartsWith("%"))
        m = new FlipFlopModule { Name = sp[0].Substring(1) };
    else if (sp[0].StartsWith("&"))
        m = new ConjunctionModule { Name = sp[0].Substring(1) };
    else
        throw new Exception();

    CommSystem.AllModules.Add(m.Name, m);
}

foreach (var line in input)
{
    var sp = line.Split(" -> ");
    var destMods = sp[1].Split(", ");
    var name = sp[0];
    if (name.StartsWith("%") || name.StartsWith("&"))
    {
        name = name.Substring(1);
    }

    foreach(var nm in destMods.Where(dm => !CommSystem.AllModules.ContainsKey(dm)))
    {
        CommSystem.AllModules.Add(nm, new DummyModule { Name = nm });
    }

    CommSystem.AllModules[name].DestModules.AddRange(destMods.Select(dm => CommSystem.AllModules[dm]));
}

var cms = CommSystem.AllModules.Values.OfType<ConjunctionModule>().ToList();
foreach(var cm in cms)
{
    foreach(var im in  CommSystem.AllModules.Values.Where(m => m.DestModules.Contains(cm)))
    {
        cm.InputModules.Add(im, Signal.LowPulse);
    }
}

for (long i = 0; i < 1000000000; i++)
{
    CommSystem.Signals.Add(new Signal { InputModule = null, DestModule = CommSystem.AllModules["broadcaster"], Pulse = null });
    CommSystem.Process(i);
}

var ret1 = CommSystem.HighPulses * CommSystem.LowPulses;
Console.WriteLine(ret1);
Console.ReadLine();

class CommSystem
{
    public static Dictionary<string, IModule> AllModules { get; set; } = new Dictionary<string, IModule>();
    public static List<Signal> Signals { get; set; } = new List<Signal>();

    public static decimal HighPulses = 0;
    public static decimal LowPulses = 0;
    //3907 ph
    //3797 vn
    //4093 kt
    //4021 hn
    public static void Process(long i)
    {
        while(Signals.Any())
        {
            var s = Signals.First();

            if(s.InputModule?.Name == "hn" && s.Pulse == Signal.HighPulse)
            {
                Console.WriteLine(i);
            }

            s.DestModule.GetPulse(s.InputModule, s.Pulse);
            if(s.Pulse == Signal.HighPulse)
            {
                HighPulses++;
            }
            else
            {
                LowPulses++;
            }
            Signals = Signals.Skip(1).ToList();
        }
    }
}

interface IModule
{
    string Name { get; set; }
    void GetPulse(IModule input, string pulse);
    List<IModule> DestModules { get; set; }
}

class Signal
{
    public const string HighPulse = "H";
    public const string LowPulse = "L";

    public IModule InputModule { get; set; }
    public IModule DestModule { get; set; }
    public string Pulse { get; set; }
}

class FlipFlopModule : IModule
{
    public bool State { get; set; } = false;
    public List<IModule> DestModules { get; set; } = new List<IModule>();
    public string Name { get; set; }

    public void GetPulse(IModule input, string pulse)
    {
        if(pulse == Signal.LowPulse)
        {
            State = !State;
            if(State)
            {
                CommSystem.Signals.AddRange(DestModules.Select(m=>new Signal { InputModule = this, DestModule = m, Pulse = Signal.HighPulse }) );
            }
            else
            {
                CommSystem.Signals.AddRange(DestModules.Select(m => new Signal { InputModule = this, DestModule = m, Pulse = Signal.LowPulse }));
            }
        }
    }
}

class ConjunctionModule : IModule
{
    public Dictionary<IModule, string> InputModules { get; set;} = new Dictionary<IModule, string>();
    public List<IModule> DestModules { get; set; } = new List<IModule>();
    public string Name { get; set; }

    public void GetPulse(IModule input, string pulse)
    {
        InputModules[input] = pulse;
        if(InputModules.Values.All(v=>v == Signal.HighPulse))
        {
            CommSystem.Signals.AddRange(DestModules.Select(m => new Signal { InputModule = this, DestModule = m, Pulse = Signal.LowPulse }));
        }
        else
        {
            CommSystem.Signals.AddRange(DestModules.Select(m => new Signal { InputModule = this, DestModule = m, Pulse = Signal.HighPulse }));
        }
    }
}

class BroadcasterModule : IModule
{
    public List<IModule> DestModules { get; set; } = new List<IModule>();
    public string Name { get; set; }
    public void GetPulse(IModule input, string pulse)
    {
        CommSystem.Signals.AddRange(DestModules.Select(m => new Signal { InputModule = this, DestModule = m, Pulse = Signal.LowPulse }));
    }
}

class DummyModule : IModule
{
    public string Name { get; set; }
    public List<IModule> DestModules { get; set; } = new List<IModule>();
    public void GetPulse(IModule input, string pulse)
    {
    }
}