﻿namespace CRUD.DAL.Abstractions;

#region << Using >>

using System.Linq.Expressions;
using Extensions;
using LinqSpecs;

#endregion

/// <summary>
///     Finds entities by specified id collection.
///     Optional: returns true if id collection is empty.
/// </summary>
public class FindEntitiesByIds<TEntity, TId> : Specification<TEntity>
        where TEntity : IId<TId>
{
    #region Properties

    private readonly TId[] ids;

    #endregion

    #region Constructors

    public FindEntitiesByIds(IEnumerable<TId> ids)
    {
        this.ids = ids.ToDistinctArrayOrEmpty();
    }

    #endregion

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        if (!this.ids.Any())
            return x => true;

        return x => this.ids.Contains(x.Id);
    }
}