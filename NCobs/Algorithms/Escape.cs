namespace NCobs.Algorithms
{
    using NByteStuff;
    using System.Collections.Generic;

    public class Escape : IByteStuff
    {
        private string EscapeString { get; set; } = "";

        private IEnumerable<string> Values { get; set; }

        public Escape(string escape, IEnumerable<string> values)
        {
            EscapeString = escape;
            Values = values;
        }

        public string Stuff(string text)
        {
            foreach (string value in Values)
            {
                text.Replace(value, EscapeString + value);
            }

            return text;
        }

        public string Unstuff(string text)
        {
            foreach (string value in Values)
            {
                text.Replace(EscapeString + value, value);
            }

            return text;
        }
    }
}
