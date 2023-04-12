namespace CRUD.Extensions;

public static class StringExt
{
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrWhitespace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static T ToEnum<T>(this string value) where T : struct, Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    public static string ToJoinedString(this IEnumerable<string> enumerable, string separator)
    {
        var enumerableArray = enumerable.ToArrayOrEmpty();

        return !enumerableArray.Any() ? string.Empty : string.Join(separator, enumerableArray);
    }
}