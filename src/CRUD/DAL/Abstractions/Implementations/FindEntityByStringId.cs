namespace CRUD.DAL.Abstractions;

#region << Using >>

using System.Linq.Expressions;
using LinqSpecs;

#endregion

/// <summary>
///     Finds an entity by specified String id.
/// </summary>
public class FindEntityByStringId<TEntity> : Specification<TEntity> where TEntity : IId<string>
{
    #region Properties

    private readonly string id;

    #endregion

    #region Constructors

    public FindEntityByStringId(string id)
    {
        this.id = id;
    }

    #endregion

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        return x => x.Id == this.id;
    }
}