namespace CRUD.Core;

public class PagingInfoDto
{
    #region Properties

    /// <summary>
    /// Starts from 1.
    /// </summary>
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalItemsCount { get; set; }

    public int PageSize { get; set; }

    #endregion
}