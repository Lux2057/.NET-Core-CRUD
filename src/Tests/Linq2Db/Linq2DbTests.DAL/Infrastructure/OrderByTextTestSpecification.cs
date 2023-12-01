namespace Linq2DbTests.DAL;

#region << Using >>

using System.Linq.Expressions;
using CRUD.DAL.Abstractions;
using Linq2DbTests.Shared;

#endregion

public class OrderByTextTestSpecification : OrderSpecification<TestEntity>
{
    #region Constructors

    public OrderByTextTestSpecification(bool isDesc) : base(isDesc) { }

    #endregion

    public override Expression<Func<TestEntity, object>> OrderExpression()
    {
        return x => x.Text;
    }
}