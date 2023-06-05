namespace CRUD.Core;

public class PaginatedResponseDto<TDto>
{
    #region Properties

    public TDto[] Items { get; set; }

    public PagingInfoDto PagingInfo { get; set; }

    #endregion
}