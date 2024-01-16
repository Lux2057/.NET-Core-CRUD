namespace CommonTests.WebAPI;

#region << Using >>

using Microsoft.AspNetCore.Http;

#endregion

public static class StringExt
{
    public static async Task<IFormFile> GetFileAsync(this string val)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(val);
        await writer.FlushAsync();
        stream.Position = 0;

        return new FormFile(stream, 0, stream.Length, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }
}