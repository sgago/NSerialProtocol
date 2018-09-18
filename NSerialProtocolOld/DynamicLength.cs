

namespace NSerialProtocol
{
    using System;

    public interface IDynamicLength
    {
        string ConvertToString(int length);
        int ConvertToInt(string value);
    }

    public class DynamicLength : IDynamicLength
    {
        public Func<int, string> ConvertIntToStringFunc { get; set; } = i =>
         {
             return i.ToString();
         };

        public Func<string, int> ConvertStringToIntFunc { get; set; } = s =>
          {
              int length = -1;

              int.TryParse(s, out length);

              return length;
          };

        public DynamicLength()
        {

        }

        public DynamicLength(Func<int, string> convertIntToStringFunc, Func<string, int> convertStringToIntFunc)
        {
            ConvertIntToStringFunc = convertIntToStringFunc;
            ConvertStringToIntFunc = convertStringToIntFunc;
        }

        public string ConvertToString(int length)
        {
            return ConvertIntToStringFunc(length);
        }

        public int ConvertToInt(string value)
        {
            return ConvertStringToIntFunc(value);
        }
    }
}
