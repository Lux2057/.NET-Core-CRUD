﻿namespace Linq2DbTests.DAL;

#region << Using >>

using System.Linq.Expressions;
using Linq2DbTests.Shared;
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