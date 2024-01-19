namespace Samples.UploadBigFile.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class FileUploadState
{
    #region Properties

    public bool IsLoading { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    FileUploadState()
    {
        IsLoading = false;
    }

    public FileUploadState(bool isLoading)
    {
        IsLoading = isLoading;
    }

    #endregion
}