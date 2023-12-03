using Day03;
var input = File.ReadAllLines("input.txt");
var ll = input.Length;
long ret1 = 0;
bool IsSymbolAdj(int line, int pos)
{
    char c;
    if(pos > 0)
    {
        c = input[line][pos - 1];
        if (c != '.' && !char.IsNumber(c))
            return true;

        if(line > 0)
        {
            c = input[line - 1][pos - 1];
            if (c != '.' && !char.IsNumber(c))
                return true;
        }

        if(line < input.Length - 1)
        {
            c = input[line + 1][pos - 1];
            if (c != '.' && !char.IsNumber(c))
                return true;
        }
    }

    if (line > 0)
    {
        c = input[line - 1][pos];
        if (c != '.' && !char.IsNumber(c))
            return true;
    }

    if (line < input.Length - 1)
    {
        c = input[line + 1][pos];
        if (c != '.' && !char.IsNumber(c))
            return true;
    }

    if (pos < ll - 1)
    {
        c = input[line][pos + 1];
        if (c != '.' && !char.IsNumber(c))
            return true;

        if (line > 0)
        {
            c = input[line - 1][pos + 1];
            if (c != '.' && !char.IsNumber(c))
                return true;
        }

        if (line < input.Length - 1)
        {
            c = input[line + 1][pos + 1];
            if (c != '.' && !char.IsNumber(c))
                return true;
        }
    }
    return false;
}

List<(int, int)> IsStarSymbolAdj(int line, int pos)
{
    List<(int, int)> ret = new List<(int, int)>();
    char c;
    if (pos > 0)
    {
        c = input[line][pos - 1];
        if (c == '*')
            ret.Add((line, pos - 1));

        if (line > 0)
        {
            c = input[line - 1][pos - 1];
            if (c == '*')
                ret.Add((line - 1, pos - 1));
        }

        if (line < input.Length - 1)
        {
            c = input[line + 1][pos - 1];
            if (c == '*')
                ret.Add((line + 1, pos - 1));
        }
    }

    if (line > 0)
    {
        c = input[line - 1][pos];
        if (c == '*')
            ret.Add((line - 1, pos));
    }

    if (line < input.Length - 1)
    {
        c = input[line + 1][pos];
        if (c == '*')
            ret.Add((line + 1, pos));
    }

    if (pos < ll - 1)
    {
        c = input[line][pos + 1];
        if (c == '*')
            ret.Add((line, pos + 1));

        if (line > 0)
        {
            c = input[line - 1][pos + 1];
            if (c == '*')
                ret.Add((line - 1, pos + 1));
        }

        if (line < input.Length - 1)
        {
            c = input[line + 1][pos + 1];
            if (c == '*')
                ret.Add((line + 1, pos + 1));
        }
    }
    return ret;
}

Dictionary<int, Dictionary<int, (int, int)>> gears = new Dictionary<int, Dictionary<int, (int, int)>>();

gears.Add(0, new Dictionary<int, (int, int)>());
for(int x = 0; x < ll; x++) { gears[0].Add(x, new(0, 1)); }

for (int i = 0; i < input.Length; i++)
{
    gears.Add(i + 1, new Dictionary<int, (int, int)>());
    gears[i + 1].Add(0, new(0, 1));
    string line = input[i];
    string num = "";
    bool adj = false;
    List<(int,int)> staradj = new List<(int, int)>();
    for(int j=0 ; j < line.Length; j++)
    {
        gears[i + 1].Add(j + 1, new(0, 1));
        var c = line[j];
        if(char.IsNumber(c))
        {
            num += c;
            if(!adj)
            {
                adj = IsSymbolAdj(i, j);
            }
                       
            foreach(var sa in IsStarSymbolAdj(i, j))
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
                    var cv = gears[sa.Item1][sa.Item2];
                    gears[sa.Item1][sa.Item2] = new(cv.Item1+1, cv.Item2 * v);
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
            var cv = gears[sa.Item1][sa.Item2];
            gears[sa.Item1][sa.Item2] = new(cv.Item1+1, cv.Item2 * v);
        }
    }
}

long ret2 = gears.SelectMany(kvp => kvp.Value).Select(kvp => kvp.Value).Where(a=>a.Item1 == 2).Select(a=>a.Item2).Sum();

Console.ReadLine();