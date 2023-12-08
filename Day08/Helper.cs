using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    public class Helper
    {
        static int gcf(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static int lcm(int a, int b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}