namespace Linq2DbTests.Core;

#region << Using >>

using LinqToDB;
using LinqToDB.Data;

#endregion

public class TestDataConnection : DataConnection
{
    #region Constructors

    public TestDataConnection(DataOptions<TestDataConnection> options) : base(options.Options) { }

    #endregion
}