namespace Tests.Models;

#region << Using >>

using System.Text.RegularExpressions;

#endregion

public static class PathHelper
{
    public static string GetApplicationRoot()
    {
        var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        if (exePath == null)
            throw new NullReferenceException();

        return new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)").Match(exePath).Value;
    }
}