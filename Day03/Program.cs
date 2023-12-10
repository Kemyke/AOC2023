using AOCHelper;

var input = File.ReadAllLines("input.txt");
var parsedInput = Helper.ParseInput(Helper.PadInput(input.ToList(), '.'));

long ret1 = 0;

for (int i = 1; i < parsedInput.Count; i++)
{
    string num = "";
    bool adj = false;
    List<Item> staradj = new List<Item>();
    for(int j=1 ; j < parsedInput[i].Count; j++)
    {
        var c = parsedInput[i][j].ValueCh;
        if(char.IsNumber(c))
        {
            num += c;
            if(!adj)
            {
                var x = Helper.AdjacentAndDiagItems(parsedInput, i, j);
                adj = x.Where(it => it.ValueCh != '.' && !char.IsNumber(it.ValueCh)).Any();
            }

            foreach (var sa in Helper.AdjacentAndDiagItems(parsedInput, i, j).Where(it => it.ValueCh == '*'))
            {
                if(!staradj.Contains(sa))
                {
                    staradj.Add(sa);
                }
            }
            
        }
        else
        {
            if (num.Length > 0)
            {
                var v = int.Parse(num);
                if (adj)
                    ret1 += v;
                foreach(var sa in staradj)
                {
                    sa.GearNum = sa.GearNum + 1;
                    sa.GearValue = sa.GearValue * v;
                }

                num = "";
                staradj.Clear();
                adj = false;
            }
        }
    }

    if (num.Length > 0)
    {
        var v = int.Parse(num);
        if (adj)
            ret1 += v;

        foreach (var sa in staradj)
        {
            sa.GearNum = sa.GearNum + 1;
            sa.GearValue = sa.GearValue * v;
        }
    }
}

long ret2 = parsedInput.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(a=>a.GearNum == 2).Select(a=>a.GearValue).Sum();

Console.ReadLine();