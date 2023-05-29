namespace CRUD.Extensions;

#region << Using >>

using System.Text.RegularExpressions;

#endregion

public static class PathHelper
{
    /// <summary>
    ///     Returns current application root path
    /// </summary>
    public static string GetApplicationRootOrDefault()
    {
        var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        return exePath == null ? string.Empty : new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)").Match(exePath).Value;
    }
}