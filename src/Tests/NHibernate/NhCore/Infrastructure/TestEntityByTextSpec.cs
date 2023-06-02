namespace EfTests.Core;

#region << Using >>

using System.Linq.Expressions;
using LinqSpecs;

#endregion

public class TestEntityByTextSpec : Specification<TestEntity>
{
    #region Properties

    private readonly string text;

    #endregion

    #region Constructors

    public TestEntityByTextSpec(string text)
    {
        this.text = text;
    }

    #endregion

    public override Expression<Func<TestEntity, bool>> ToExpression()
    {
        return x => x.Text == this.text;
    }
}