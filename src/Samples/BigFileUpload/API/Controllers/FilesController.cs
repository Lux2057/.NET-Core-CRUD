namespace Samples.UploadBigFile.API;

#region << Using >>

using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;
using Samples.UploadBigFile.Shared;

#endregion

[Route("[controller]/[action]")]
public class FilesController : ControllerBase
{
    #region Properties

    readonly IChunksStorageService _chunksStorageService;

    #endregion

    #region Constructors

    public FilesController(IChunksStorageService chunksStorageService)
    {
        this._chunksStorageService = chunksStorageService;
    }

    #endregion

    [Route("~/" + ApiRoutes.UploadFile)]
    [HttpPost]
    [DisableRequestSizeLimit]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UploadFile([FromForm(Name = ApiRoutes.Params.Data)] IFormFile data,
                                                [FromForm(Name = ApiRoutes.Params.Order)]
                                                int order,
                                                [FromForm(Name = ApiRoutes.Params.IsLast)]
                                                bool isLast,
                                                [FromForm(Name = ApiRoutes.Params.UID)]
                                                string uid)
    {
        var chunk = await data.ToByteArrayAsync();

        this._chunksStorageService.AddChunk(uid: uid,
                                            order: order,
                                            chunk: chunk);

        if (!isLast)
            return Ok();

        var file = this._chunksStorageService.PopItem(uid);

        return Ok(file);
    }
}