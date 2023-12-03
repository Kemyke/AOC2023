using Day03;

var input = File.ReadAllLines("input.txt");
var parsedInput = Helper.ParseInputWithPadding(input.ToList(), new Item { ValueCh = '.', ValueStr = ".", GearNum = 0, GearValue = 1 });

long ret1 = 0;

for (int i = 0; i < input.Length; i++)
{
    string line = input[i];
    string num = "";
    bool adj = false;
    List<Item> staradj = new List<Item>();
    for(int j=0 ; j < line.Length; j++)
    {
        var c = line[j];
        if(char.IsNumber(c))
        {
            num += c;
            if(!adj)
            {
                adj = Helper.AdjacentAndDiag(parsedInput, i, j).Where(it => it.ValueCh != '.' && !char.IsNumber(it.ValueCh)).Any();
            }

            foreach (var sa in Helper.AdjacentAndDiag(parsedInput, i, j).Where(it => it.ValueCh == '*'))
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