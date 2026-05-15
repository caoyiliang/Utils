using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace Utils;

public static class StringUtils
{
    public static List<decimal> GetAllNum(this string str)
    {
        if (str is null) throw new ArgumentNullException(nameof(str));
        var result = new List<decimal>();
        var stringBuilder = new StringBuilder();
        foreach (var item in str)
        {
            if (item.IsDigitOrDot())
            {
                stringBuilder.Append(item);
            }
            else if (item.IsNegative())
            {
                if (stringBuilder.Length > 0)
                {
                    var token = stringBuilder.ToString();
                    if (decimal.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out var num))
                        result.Add(num);
                    stringBuilder.Clear();
                }
                stringBuilder.Append(item);
            }
            else
            {
                if (stringBuilder.Length > 0)
                {
                    var token = stringBuilder.ToString();
                    if (decimal.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out var num))
                        result.Add(num);
                    stringBuilder.Clear();
                }
            }
        }
        if (stringBuilder.Length > 0)
        {
            var token = stringBuilder.ToString();
            if (decimal.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out var num))
                result.Add(num);
            stringBuilder.Clear();
        }
        return result;
    }

    private static bool IsDigitOrDot(this char c)
    {
        return char.IsDigit(c) || c == '.';
    }

    public static bool IsNegative(this char c)
    {
        return c == '-';
    }

    /// <summary>类转Uri参数</summary>
    /// <param name="obj">类</param>
    /// <param name="sort">排序</param>
    /// <param name="url">地址</param>
    /// <param name="urlEncode">是否转UrlString</param>
    public static string ModelToUriParam(this object obj, bool sort = true, string url = "", bool urlEncode = false)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        PropertyInfo[] properties = obj.GetType().GetProperties();
        if (properties.Length == 0)
        {
            return url;
        }
        var sb = new StringBuilder();
        sb.Append(url);
        if (url != "") sb.Append('?');
        if (sort) properties = properties.OrderBy(x => x.Name).ToArray();
        int added = 0;
        foreach (var p in properties)
        {
            var v = p.GetValue(obj, null);
            if (v == null)
                continue;

            sb.Append(p.Name);
            sb.Append('=');
            var vs = v.ToString() ?? string.Empty;
            sb.Append(urlEncode ? HttpUtility.UrlEncode(vs) : vs);
            sb.Append('&');
            added++;
        }
        if (added > 0)
        {
            sb.Remove(sb.Length - 1, 1);
        }

        return sb.ToString();
    }

    /// <summary>主机名解析</summary>
    /// <exception cref="ArgumentException">DNS 解析失败</exception>
    public static async Task<IPAddress> ResolveHostAsync(this string hostOrIp)
    {
        if (IPAddress.TryParse(hostOrIp, out var ipAddress))
        {
            return ipAddress;
        }

        try
        {
            var hostEntry = await Dns.GetHostEntryAsync(hostOrIp);
            if (hostEntry.AddressList.Length == 0)
                throw new InvalidOperationException($"DNS 解析成功但无可用地址: {hostOrIp}");
            return hostEntry.AddressList[0];
        }
        catch (SocketException ex)
        {
            throw new InvalidOperationException($"DNS 解析失败: {ex.Message}", ex);
        }
    }
}
