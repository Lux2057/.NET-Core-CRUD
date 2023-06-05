namespace EfTests.DAL;

#region << Using >>

using System.Linq.Expressions;
using EfTests.Shared;
using LinqSpecs;

#endregion

public class TestByTextSpecification : Specification<TestEntity>
{
    #region Properties

    public readonly string text;

    #endregion

    #region Constructors

    public TestByTextSpecification(string text)
    {
        this.text = text;
    }

    #endregion

    public override Expression<Func<TestEntity, bool>> ToExpression()
    {
        return x => x.Text == this.text;
    }
}