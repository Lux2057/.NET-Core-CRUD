namespace CRUD.Core;

#region << Using >>

using System;

#endregion

public class PaginatedResponseDto<TDto>
{
    #region Properties

    public TDto[] Items { get; set; }

    public PagingInfoDto PagingInfo { get; set; }

    #endregion

    #region Constructors

    public PaginatedResponseDto()
    {
        Items = Array.Empty<TDto>();
        PagingInfo = new PagingInfoDto
                     {
                             CurrentPage = 1,
                             PageSize = 1,
                             TotalItemsCount = 0,
                             TotalPages = 1
                     };
    }

    #endregion
}