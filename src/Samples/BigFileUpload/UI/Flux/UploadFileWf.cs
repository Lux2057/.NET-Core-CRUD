namespace Samples.UploadBigFile.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.UploadBigFile.Shared;

#endregion

public class SetIsLoadingStatusWf
{
    #region Nested Classes

    public record Init(bool Status);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static FileUploadState OnSet(FileUploadState state, Init action)
    {
        return new FileUploadState(isLoading: action.Status);
    }
}