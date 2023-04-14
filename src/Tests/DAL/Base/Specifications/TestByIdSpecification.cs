namespace Tests.DAL;

#region << Using >>

using System.Linq.Expressions;
using LinqSpecs;

#endregion

public class TestByIdSpecification : Specification<TestEntity>
{
    #region Properties

    public readonly int id;

    #endregion

    #region Constructors

    public TestByIdSpecification(int id)
    {
        this.id = id;
    }

    #endregion

    public override Expression<Func<TestEntity, bool>> ToExpression()
    {
        return x => x.Id == this.id;
    }
}