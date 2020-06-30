using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class StringUtils
    {
        public static List<decimal> GetAllNum(this string str)
        {
            var result = new List<decimal>();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in str)
            {
                if (item.lsNum())
                {
                    stringBuilder.Append(item);
                }
                else if (item.IsNegative())
                {
                    if (stringBuilder.Length > 0)
                    {
                        result.Add(decimal.Parse(stringBuilder.ToString()));
                        stringBuilder.Clear();
                    }
                    stringBuilder.Append(item);
                }
                else
                {
                    if (stringBuilder.Length > 0)
                    {
                        result.Add(decimal.Parse(stringBuilder.ToString()));
                        stringBuilder.Clear();
                    }
                }
            }
            if (stringBuilder.Length > 0)
            {
                result.Add(decimal.Parse(stringBuilder.ToString()));
                stringBuilder.Clear();
            }
            return result;
        }
        private static bool lsNum(this char c)
        {
            return c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '.';
        }
        public static bool IsNegative(this char c)
        {
            return c == '-';
        }
    }
}
