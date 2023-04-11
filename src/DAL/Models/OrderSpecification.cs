namespace CRUD.DAL;

#region << Using >>

using System;
using System.Linq.Expressions;

#endregion

public record OrderSpecification<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> Expression, OrderType Type = OrderType.Ascending);