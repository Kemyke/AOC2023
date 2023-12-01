var input = File.ReadAllLines("input.txt");
var s = input.Select(l => l.Replace("one", "o1ne").Replace("two", "tw2o").Replace("three", "th3ree")
                    .Replace("four", "fo4ur").Replace("five", "fi5ve").Replace("six", "s6ix").Replace("seven", "se7ven")
                    .Replace("eight", "eig8ht").Replace("nine", "ni9ne").Replace("zero", "ze0ro"));

var ret1 = input.Select(l => l.Where(c => char.IsNumber(c)).ToList())
                .Select(n => long.Parse(n.First().ToString()) * 10 + long.Parse(n.Last().ToString()))
                .Sum();

var ret2 = s.Select(l => l.Where(c => char.IsNumber(c)).ToList())
                .Select(n => long.Parse(n.First().ToString()) * 10 + long.Parse(n.Last().ToString()))
                .Sum();

Console.ReadLine();