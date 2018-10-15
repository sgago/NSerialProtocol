using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionTrees
{
    class Program
    {
        public static bool HasEndFlag = true;
        public static string EndFlag { get; private set; } = "\n";

        static void Main(string[] args)
        {

            Evaluate("abc\n123");

        }

        static void Evaluate(string data)
        {
            Regex re;
            Expression e;
            List<string> parts;

            if (HasEndFlag && data.Contains(EndFlag))
            {
                re = new Regex(string.Format(@"((?!{0}).)*{0}", EndFlag));

                parts = re.Matches(data)
                    .Cast<Match>()
                    .Select(x => x.Value)
                    .ToList();
            }
        }
    }
}
