namespace CRUD.DAL;

#region << Using >>

using System;
using System.Linq.Expressions;

#endregion

public abstract class OrderSpecification<TEntity>
{
    #region Properties

    public bool IsDesc { get; }

    #endregion

    #region Constructors

    protected OrderSpecification(bool isDesc)
    {
        IsDesc = isDesc;
    }

    #endregion

    public abstract Expression<Func<TEntity, object>> OrderExpression();
}