var input = File.ReadAllLines("input.txt").ToList();

List<long> seeds = input[0].Replace("seeds: ", "").Split(" ").Select(s=>long.Parse(s)).ToList();
List<(long, long)> seedRanges = seeds.Where((s,i) => i % 2 == 0).Select((s, i) => (s, i)).Select(t => (t.s, seeds[t.i * 2 + 1])).ToList();

List<Interval> seedToSoil = new List<Interval>();
List<Interval> soilToFer = new List<Interval>();
List<Interval> ferToWater = new List<Interval>();
List<Interval> waterToLight = new List<Interval>();
List<Interval> lightToTemp = new List<Interval>();
List<Interval> tempToHumid = new List<Interval>();
List<Interval> humidToLoc = new List<Interval>();

Parse("seed-to-soil map:", input, seedToSoil);
Parse("soil-to-fertilizer map:", input, soilToFer);
Parse("fertilizer-to-water map:", input, ferToWater);
Parse("water-to-light map:", input, waterToLight);
Parse("light-to-temperature map:", input, lightToTemp);
Parse("temperature-to-humidity map:", input, tempToHumid);
Parse("humidity-to-location map:", input, humidToLoc);

List<long> locations = new List<long>();
List<long> minSteps = new List<long>();

foreach (var seed in seedRanges)
{
    long cs = seed.Item1;
    while (cs < seed.Item1 + seed.Item2)
    {
        long mv;
        mv = GetMappedValueWithDiff(cs, seedToSoil, minSteps);
        mv = GetMappedValueWithDiff(mv, soilToFer, minSteps);
        mv = GetMappedValueWithDiff(mv, ferToWater, minSteps);
        mv = GetMappedValueWithDiff(mv, waterToLight, minSteps);
        mv = GetMappedValueWithDiff(mv, lightToTemp, minSteps);
        mv = GetMappedValueWithDiff(mv, tempToHumid, minSteps);
        mv = GetMappedValueWithDiff(mv, humidToLoc, minSteps);

        locations.Add(mv);

        cs = cs + minSteps.Min();
        minSteps.Clear();
    }
}


var ret2 = locations.Min();
Console.ReadLine();

void Parse(string title, List<string> input, List<Interval> output)
{
    var stss = input.SkipWhile(s => s != title).Skip(1).TakeWhile(s => s != "").ToList()
                .Select(l => l.Split(" ").Select(s => long.Parse(s)).ToList());
    foreach (var lv in stss)
    {
        output.Add(new Interval { Start = lv[1], End = lv[1] + lv[2] - 1, MappedValue = lv[0], Idx = output.Count });
    }
}


long GetMappedValue(long value, List<Interval> intervals)
{
    var b = intervals.SingleOrDefault(iv => iv.End >= value && iv.Start <= value);
    if(b != null)
    {
        return b.MappedValue + (value - b.Start);
    }
    return value;
}

long GetMappedValueWithDiff(long value, List<Interval> intervals, List<long> minStep)
{
    var b = intervals.SingleOrDefault(iv => iv.End >= value && iv.Start <= value);
    if (b != null)
    {
        minStep.Add(b.End - value + 1);
        return b.MappedValue + (value - b.Start);
    }
    //minStep.Add(1);
    return value;
}

class Interval
{
    public int Idx { get; set; }
    public long Start { get; set; }
    public long End { get; set; }
    public long MappedValue { get; set; }
}