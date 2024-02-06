namespace HotelsManager.Extensions;

using System.Text;

public static class EnumerableExtensions
{
    public static string GetHtml<T>(this IEnumerable<T> hotels)
    {
        Type type = typeof(T);

        var props = type.GetProperties();

        StringBuilder sb = new StringBuilder(100);
        sb.Append("<ul>");

        foreach (var hotel in hotels)
        {
            foreach (var prop in props)
            {
                sb.Append($"<li><span>{prop.Name}: </span>{prop.GetValue(hotel)}</li>");
            }
            sb.Append("<br/>");
        }
        sb.Append("</ul>");

        return sb.ToString();
    }
}