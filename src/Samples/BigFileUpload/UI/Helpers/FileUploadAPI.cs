namespace Samples.UploadBigFile.UI;

#region << Using >>

using System.Net.Http.Headers;
using Samples.UploadBigFile.Shared;

#endregion

public class FileUploadAPI
{
    #region Properties

    private readonly HttpClient Http;

    #endregion

    #region Constructors

    public FileUploadAPI(HttpClient http)
    {
        this.Http = http;
    }

    #endregion

    public async Task<HttpResponseMessage> UploadFileChunkAsync(string uid,
                                                                int order,
                                                                byte[] data,
                                                                string fileName,
                                                                bool isLast)
    {
        using (var content = new MultipartFormDataContent())
        {
            var fileContent = new StreamContent(new MemoryStream(data));

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            content.Add(content: fileContent,
                        name: ApiRoutes.Params.Data,
                        fileName: fileName);

            content.Add(content: new StringContent(uid),
                        name: ApiRoutes.Params.UID);

            content.Add(content: new StringContent(order.ToString()),
                        name: ApiRoutes.Params.Order);

            content.Add(content: new StringContent(isLast.ToString()),
                        name: ApiRoutes.Params.IsLast);

            return await this.Http.PostAsync(ApiRoutes.UploadFile, content);
        }
    }
}