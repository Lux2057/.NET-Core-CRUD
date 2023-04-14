namespace Tests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL;

#endregion

public class EfTransactionScopeAsyncTests : EfUnitOfWorkTest
{
    #region Constructors

    public EfTransactionScopeAsyncTests(IUnitOfWork unitOfWork, TestDbContext context)
            : base(unitOfWork, context) { }

    #endregion

    [Fact]
    public async Task Should_open_and_close_a_transaction_once()
    {
        var transactionId1 = await this.unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadCommitted);
        var transactionId2 = await this.unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadUncommitted);

        Assert.NotNull(this.context.Database.CurrentTransaction);
        Assert.Equal(transactionId1, this.context.Database.CurrentTransaction.TransactionId.ToString());
        Assert.Equal(string.Empty, transactionId2);

        await this.unitOfWork.EndTransactionScopeAsync(transactionId1);
        await this.unitOfWork.EndTransactionScopeAsync(transactionId2);

        Assert.True(this.context.Database.CurrentTransaction == null);
    }
}