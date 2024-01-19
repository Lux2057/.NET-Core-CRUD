namespace Samples.UploadBigFile.UI.Pages;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

#endregion

[Route("")]
[UsedImplicitly]
public partial class FileUploadPage : Fluxor.Blazor.Web.Components.FluxorComponent
{
    #region Constants

    const long defaultChunkSize = 5 * 1024 * 1024; //5 Mb

    private const int uploadRetryCount = 3;

    #endregion

    #region Properties

    [Inject]
    IDispatcher Dispatcher { get; set; }

    [Inject]
    IState<FileUploadState> State { get; set; }

    [Inject]
    private HttpClient Http { get; set; }

    #endregion

    private async Task UploadFiles(InputFileChangeEventArgs args)
    {
        Dispatcher.Dispatch(new SetIsLoadingStatusWf.Init(true));

        var files = args.GetMultipleFiles(int.MaxValue);
        var api = new FileUploadAPI(Http);

        foreach (var file in files)
        {
            var uid = Guid.NewGuid().ToString("N");
            long uploadedBytes = 0;
            var totalBytes = file.Size;
            var isLast = false;

            await using (var inStream = file.OpenReadStream(long.MaxValue))
            {
                var order = 0;
                while (!isLast)
                {
                    var chunkSize = defaultChunkSize;

                    if (uploadedBytes + defaultChunkSize > totalBytes)
                        chunkSize = totalBytes - uploadedBytes;

                    var chunk = new byte[chunkSize];
                    try
                    {
                        await inStream.ReadAsync(chunk, 0, chunk.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    uploadedBytes += chunkSize;
                    isLast = uploadedBytes == totalBytes;

                    var counter = 0;
                    while (counter < uploadRetryCount)
                    {
                        var chunkUploadResult = await api.UploadFileChunkAsync(uid: uid,
                                                                               order: order,
                                                                               data: chunk,
                                                                               isLast: isLast,
                                                                               fileName: file.Name);

                        if (chunkUploadResult.IsSuccessStatusCode)
                            break;

                        counter++;
                    }

                    if (counter == uploadRetryCount)
                        throw new Exception();

                    order++;
                }
            }
        }

        Dispatcher.Dispatch(new SetIsLoadingStatusWf.Init(false));
    }
}