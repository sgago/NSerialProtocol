using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpTree
{
    class Program
    {
        static void Main(string[] args)
        {
            string startFlag = "|";
            string receivedData = "garbage|data";

            if (receivedData.Contains(startFlag))
            {
                //var parts = receivedData.Split(@"{0}((?!{0})[\s\S]){2}");

                //Expression startFlagExpression = 
                    
            }
        }

        public decimal Evaluate(string expression)
        {
            if (expression.Contains('+'))
            {
                var parts = expression.Split('+');
                Expression sum = Expression.Add(
                       Expression.Constant(decimal.Parse(parts[0])),
                       Expression.Constant(decimal.Parse(parts[1])));
                var lambda = Expression.Lambda<Func<decimal>>(sum);
                var compiled = lambda.Compile();
                return compiled();
            }

            decimal value = 0;
            decimal.TryParse(expression, out value);
            return value;
        }
    }
}
