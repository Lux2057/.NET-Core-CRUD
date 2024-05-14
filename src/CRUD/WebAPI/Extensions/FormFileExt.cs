namespace CRUD.WebAPI;

#region << Using >>

using Microsoft.AspNetCore.Http;

#endregion

public static class FormFileExt
{
    public static async Task<byte[]> ToByteArrayAsync(this IFormFile file,
                                                      CancellationToken cancellationToken = default)
    {
        byte[] data;
        await using (var inStream = file.OpenReadStream())
        {
            data = new byte[file.Length];
            var bs = await inStream.ReadAsync(data, 0, data.Length, cancellationToken);
        }

        return data;
    }
}