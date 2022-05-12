using System.Reflection;
using System.Text;
using System.Web;

namespace Utils
{
    public static class StringUtils
    {
        public static List<decimal> GetAllNum(this string str)
        {
            var result = new List<decimal>();
            var stringBuilder = new StringBuilder();
            foreach (var item in str)
            {
                if (item.LsNum())
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
        private static bool LsNum(this char c)
        {
            return c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '.';
        }
        public static bool IsNegative(this char c)
        {
            return c == '-';
        }

        /// <summary>
        /// 类转Uri参数
        /// </summary>
        /// <param name="obj">类</param>
        /// <param name="sort">排序</param>
        /// <param name="url">地址</param>
        /// <param name="urlEncode">是否转UrlString</param>
        public static string ModelToUriParam(this object obj, bool sort = true, string url = "", bool urlEncode = false)
        {
            PropertyInfo[] propertis = obj.GetType().GetProperties();
            var sb = new StringBuilder();
            sb.Append(url);
            if (url != "") sb.Append('?');
            if (sort) propertis = propertis.OrderBy(x => x.Name).ToArray();
            foreach (var p in propertis)
            {
                var v = p.GetValue(obj, null);
                if (v == null)
                    continue;

                sb.Append(p.Name);
                sb.Append('=');
                sb.Append(urlEncode ? HttpUtility.UrlEncode(v.ToString()) : v.ToString());
                sb.Append('&');
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}
