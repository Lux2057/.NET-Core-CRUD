namespace NhTests.DAL;

#region << Using >>

using System.Linq.Expressions;
using CRUD.DAL.Abstractions;
using NhTests.Shared;

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