var input = File.ReadAllLines("input.txt");
var times = input[0].Replace("Time:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(v => int.Parse(v)).ToList();
var distances = input[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(v => int.Parse(v)).ToList();

var games = times.Zip(distances).ToList();
int ret1 = 1;

foreach(var game in games)
{
    int pr = 0;
    for(int i = 0; i <= game.First; i++)
    {
        if((game.First - i) * (i) > game.Second)
        {
            pr++;
        }
    }
    ret1 *= pr;
}

var time = long.Parse(input[0].Replace("Time:", "").Replace(" ", ""));
var distance = long.Parse(input[1].Replace("Distance:", "").Replace(" ", ""));
var ret2 = 0;
for (long i = 0; i <= time; i++)
{
    if ((time - i) * (i) > distance)
    {
        ret2++;
    }
}


Console.ReadLine();