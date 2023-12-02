using Day02;
var input = File.ReadAllLines("input.txt");
int ret1 = 0;
long ret2 = 0;
foreach(var line in input)
{
    var sp = line.Split(":");
    var gameid = int.Parse(sp[0].Replace("Game ", ""));
    var outcomes = sp[1].Split(";");
    bool possible = true;
    int minred = 0;
    int mingreen = 0;
    int minblue = 0;

    foreach(var outcome in outcomes)
    {
        var results = outcome.Trim().Split(",").Select(o => o.Trim().Split(" ")).ToList();
        var red = results.Where(r => r[1] == "red").Select(r => int.Parse(r[0])).SingleOrDefault();
        var green = results.Where(r => r[1] == "green").Select(r => int.Parse(r[0])).SingleOrDefault();
        var blue = results.Where(r => r[1] == "blue").Select(r => int.Parse(r[0])).SingleOrDefault();
        if(red > minred)
        {
            minred = red;
        }
        if (green > mingreen)
        {
            mingreen = green;
        }
        if (blue > minblue)
        {
            minblue = blue;
        }
        if (red > 12 || green > 13 || blue > 14)
        {
            possible = false;
        }
    }
    if(possible)
    {
        ret1 += gameid;
    }
    var power = minred * mingreen * minblue;
    ret2 += power;
}

Console.ReadLine();