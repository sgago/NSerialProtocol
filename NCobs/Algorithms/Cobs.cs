namespace NByteStuff.Algorithms
{
    using NCobs.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // 1. Add an overhead byte which is 1 plus the number of non-zero bytes that follow
    // 2. If we encounter a zero, replace it with N + the number of non-zero bytes that follow
    // Note: We should alert the user if they use cobs and ASCII (aka only 7 bits, need 8).
    // ERR: If user selects (char)254 as delimiter, this will fail.

    /// <summary>
    /// The consistent overhead byte stuffing (COBS) encoding and decoding algorithms
    /// for removing all zero characters from text so zero can be used for demarcation.
    /// </summary>
    public class Cobs : IByteStuff
    {
        /// <summary>
        /// The maximum length of a string section without a zero.
        /// </summary>
        private const int MaxSectionLength = 0xFF;

        /// <summary>
        /// Instantiates a new instance of the consistent overhead
        /// byte stuffing (COBS) class which replaces all instances
        /// of the null, zero, or '\0' character.
        /// </summary>
        public Cobs()
        {
        }

        public string Stuff(string text)
        {
            string result = text;

            if (!string.IsNullOrEmpty(text))
            {
                List<string> newLines = null;
                List<string> lines = text.Split(new char[] { char.MinValue },
                    StringSplitOptions.None).ToList();

                // First, make sure none of the lines are greater than 254 characters.
                // If they are, we need to break them down into strings less than 255.
                for (int i = 0; i < lines.Count; i++)
                {
                    // Split lines[i] into strings with a max length of 254 characters
                    // (No section can exceed 254 characters)
                    newLines = lines[i].Split(MaxSectionLength - 1).ToList();

                    // Did we create any new lines? (Was lines[i] > 254 characters?)
                    if (newLines.Count > 0)
                    {
                        // Need to remove the lines[i] because we're replacing it
                        // with the split lines
                        lines.RemoveAt(i);

                        // Insert the new lines
                        lines.InsertRange(i, newLines);

                        // Update the iterator
                        i += newLines.Count - 1;
                    }
                }

                for (int i = 0; i < lines.Count; i++)
                {
                    // Is this line less than MaxLength?
                    if (lines[i].Length < MaxSectionLength)
                    {
                        // Line ends with a 0, byte stuff it
                        lines[i] = (char)(lines[i].Length + 1) + lines[i];
                    }
                    else
                    {
                        // Line is big, add 0xFF infront of it
                        lines[i] = (char)MaxSectionLength + lines[i];
                    }
                }

                // Join all the lines back together
                result = string.Concat(lines);
            }

            return result;
        }

        public string Unstuff(string text)
        {
            int index = 0;
            int prev = 0;
            string result = text;
            StringBuilder stringBuilder = null;

            if (!string.IsNullOrEmpty(text))
            {
                stringBuilder = new StringBuilder(text);

                while (index < stringBuilder.Length)
                {
                    // Get the next index
                    index = stringBuilder[index] + index;

                    // Is this section less than the maxium size?
                    if (stringBuilder[prev] < MaxSectionLength)
                    {
                        // Put the zero at the end of the section
                        stringBuilder[prev] = char.MinValue;
                    }
                    else
                    {
                        // Remove the overhead byte for a section
                        // greater than 254 characters.
                        stringBuilder.Remove(prev, 1);
                    }

                    prev = index;
                }

                // Remove the overhead byte at the very start
                stringBuilder.Remove(0, 1);

                result = stringBuilder.ToString();
            }

            return result;
        }
    }
}
