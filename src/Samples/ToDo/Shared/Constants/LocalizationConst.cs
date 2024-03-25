namespace Samples.ToDo.Shared;

#region << Using >>

#endregion

public class LocalizationConst
{
    #region Constants

    public const string DefaultLanguage = "en-US";

    public static readonly Dictionary<string, string> SupportedLanguages =
            new()
            {
                    { "en-US", "English" },
                    { "ru-RU", "Русский" }
            };

    #endregion
}