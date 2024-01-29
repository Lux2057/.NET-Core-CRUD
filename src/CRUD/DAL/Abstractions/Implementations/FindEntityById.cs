namespace CRUD.DAL.Abstractions;

#region << Using >>

using System.Linq.Expressions;
using LinqSpecs;

#endregion

/// <summary>
///     Finds an entity by specified Int id.
/// </summary>
public class FindEntityByIntId<TEntity> : Specification<TEntity> where TEntity : IId<int>
{
    #region Properties

    private readonly int id;

    #endregion

    #region Constructors

    public FindEntityByIntId(int id)
    {
        this.id = id;
    }

    #endregion

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        return x => x.Id == this.id;
    }
}