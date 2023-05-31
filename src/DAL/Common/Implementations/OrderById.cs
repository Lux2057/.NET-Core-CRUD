namespace CRUD.DAL;

#region << Using >>

using System;
using System.Linq.Expressions;

#endregion

public class OrderById<TEntity, TId> : OrderSpecification<TEntity> where TEntity : IId<TId>
{
    #region Constructors

    public OrderById(bool isDesc) : base(isDesc) { }

    #endregion

    public override Expression<Func<TEntity, object>> OrderExpression()
    {
        return x => x.Id;
    }
}