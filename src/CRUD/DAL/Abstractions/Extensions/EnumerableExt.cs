namespace CRUD.DAL.Abstractions;

#region << Using >>

using Extensions;

#endregion

public static class EnumerableExt
{
    /// <summary>
    ///     Returns an distinct array that contains Ids from the input sequence or
    ///     an empty array in case when the sequence is null or empty.
    /// </summary>
    public static TId[] GetIds<TEntity, TId>(this IEnumerable<TEntity> entities) where TEntity : IId<TId>
    {
        return (entities?.Select(r => r.Id)).ToDistinctArrayOrEmpty();
    }
}