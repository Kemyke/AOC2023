using Day10;

var input = File.ReadAllLines("input.txt").ToList();

var map = Helper.ParseInput(input);
var sp = map.SelectMany(m => m.Value).Single(kv => kv.Value.ValueStr == "S").Value;
map[sp.Y][sp.X].ValueStr = "|";
map[sp.Y][sp.X].ValueCh = '|';

var ret1 = 0;
var cp = map[sp.Y][sp.X];
HashSet<Item> steps = new HashSet<Item> { cp };
while(true)
{
    var moves = cp.Moves();
    var nm = moves.Where(m => !steps.Contains(map[m.Item2][m.Item1]));

    if(!nm.Any())
    {
        break;
    }
    var nc = nm.First();
    cp = map[nc.Item2][nc.Item1];
    steps.Add(cp);

    ret1++;
}

ret1 = (ret1 + 1) / 2;


for (int y = 0; y < map.Count; y++)
{
    for (int x = 0; x < map[0].Count; x++)
    {
        var item = map[y][x];
        if (!steps.Contains(item) && item.ValueCh != '.')
        {
            item.ValueStr = ".";
            item.ValueCh = '.';
        }
    }
}

var ret2 = 0;
for (int y = 0; y < map.Count; y++)
{
    var lc = 0;
    char lastcorner = ' ';
    for (int x = 0; x < map[0].Count; x++)
    {
        var item = map[y][x];
        if (item.ValueCh == 'X')
            continue;
        if (item.ValueCh != '.')
        {
            if (item.ValueCh == '-')
                continue;
            if (item.ValueCh == '|')
                lc++;
            else if (lastcorner == ' ')
                lastcorner = item.ValueCh;
            else
            {
                if (lastcorner == 'F')
                {
                    if (item.ValueCh == 'J')
                        lc++;
                    else if (item.ValueCh == '7')
                        lc += 2;
                    else
                        throw new Exception();
                    lastcorner = ' ';
                }
                else if (lastcorner == 'L')
                {
                    if(item.ValueCh == '7')
                        lc++;
                    else if (item.ValueCh == 'J')
                        lc += 2;
                    else
                        throw new Exception();
                    lastcorner = ' ';
                }
            }
        }
        else if (lc % 2 == 1)
        {
            ret2++;
        }
    }
}
Console.ReadLine();
