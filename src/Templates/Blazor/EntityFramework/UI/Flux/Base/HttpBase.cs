namespace Templates.Blazor.EF.UI;

public abstract class HttpBase
{
    #region Properties

    protected readonly HttpClient Http;

    #endregion

    #region Constructors

    public HttpBase(HttpClient http)
    {
        this.Http = http;
    }

    #endregion
}