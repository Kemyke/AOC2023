var input = File.ReadAllLines("input.txt").ToList();

var maps = new List<List<string>>();
long ret2 = 0;

List<string> m = new List<string>();
foreach (var line in input)
{
    if(line != "")
        m.Add(line);
    else
    {
        maps.Add(m);
        m = new List<string>();
    }
}
maps.Add(m);

foreach(var map in maps)
{
    if(!FindHorizontal(map))
    {
        FindVertical(map);
    }
}

Console.ReadLine();

bool IsHorizontalReflectionWithSmudge(List<string> map, int idx)
{
    int diff = 0;
    bool ret = true;
    int j = 1;
    for(int i = idx - 1; i >= 0; i--)
    {
        if(idx + 1 + j >= map.Count)
        {
            break;
        }

        if (map[i] != map[idx + 1 + j])
        {
            if(diff == 1)
            {
                ret = false;
                break;
            }
            if(Distance(map[i], map[idx + 1 + j]) == 1)
            {
                diff = 1;
            }
            else
            {
                ret = false;
                break;
            }
        }
        j++;
    }
    if(diff == 0)
    {
        ret = false;
    }
    return ret;
}

bool IsHorizontalReflection(List<string> map, int idx)
{
    bool ret = true;
    int j = 1;
    for (int i = idx - 1; i >= 0; i--)
    {
        if (idx + 1 + j >= map.Count)
        {
            break;
        }

        if (map[i] != map[idx + 1 + j])
        {
                ret = false;
                break;
        }
        j++;
    }
    return ret;
}


bool IsVerticalReflection(List<string> map, int idx)
{
    bool ret = true;
    int j = 1;
    for (int i = idx - 1; i >= 0; i--)
    {
        if (idx + 1 + j >= map[0].Length)
        {
            break;
        }

        if (GetVertString(map, i) != GetVertString(map, idx + 1 + j))
        {
            ret = false;
            break;
        }
        j++;
    }
    return ret;
}

bool IsVerticalReflectionWithSmudge(List<string> map, int idx)
{
    int diff = 0;
    bool ret = true;
    int j = 1;
    for (int i = idx - 1; i >= 0; i--)
    {
        if (idx + 1 + j >= map[0].Length)
        {
            break;
        }

        if (GetVertString(map, i) != GetVertString(map, idx + 1 + j))
        {
            if (diff == 1)
            {
                ret = false;
                break;
            }
            if (Distance(GetVertString(map, i), GetVertString(map, idx + 1 + j)) == 1)
            {
                diff = 1;
            }
            else
            {
                ret = false;
                break;
            }
        }
        j++;
    }
    if (diff == 0)
    {
        ret = false;
    }

    return ret;
}

bool FindHorizontal(List<string> map)
{
    for(int i = 0; i < map.Count - 1; i++)
    {
        if (map[i] == map[i+1])
        {
            if(IsHorizontalReflectionWithSmudge(map, i))
            {
                ret2 += 100 * (i + 1);
                return true;
            }
        }
        else if (Distance(map[i], map[i+1]) == 1)
        {
            if (IsHorizontalReflection(map, i))
            {
                ret2 += 100 * (i + 1);
                return true;
            }
        }
    }
    return false;
}

string GetVertString(List<string> map, int idx)
{
    return new string(map.Select(m => m[idx]).ToArray());
}

bool FindVertical(List<string> map)
{
    for (int i = 0; i < map[0].Length - 1; i++)
    {
        if (GetVertString(map,i) == GetVertString(map, i + 1))
        {
            if (IsVerticalReflectionWithSmudge(map, i))
            {
                ret2 += (i + 1);
                return true;
            }
        }
        else if (Distance(GetVertString(map, i), GetVertString(map,i + 1)) == 1)
        {
            if (IsVerticalReflection(map, i))
            {
                ret2 += (i + 1);
                return true;
            }
        }
    }
    return false;
}

int Distance(string firstStrand, string secondStrand)
{
    if (firstStrand.Length != secondStrand.Length) { throw new Exception(); }

    return firstStrand.Zip(secondStrand, (c, b) => c != b).Count(f => f == true);
}