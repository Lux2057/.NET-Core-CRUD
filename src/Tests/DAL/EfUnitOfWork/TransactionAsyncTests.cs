namespace Tests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL;

#endregion

public class TransactionAsyncTests : EfUnitOfWorkTest
{
    #region Constructors

    public TransactionAsyncTests(IUnitOfWork unitOfWork, TestDbContext context)
            : base(unitOfWork, context) { }

    #endregion

    [Fact]
    public async Task Should_open_and_close_a_transaction_once()
    {
        await this.unitOfWork.BeginTransactionAsync(PermissionType.Read);
        await this.unitOfWork.BeginTransactionAsync(PermissionType.ReadWrite);

        Assert.NotNull(this.context.Database.CurrentTransaction);

        await this.unitOfWork.EndTransactionAsync();
        await this.unitOfWork.EndTransactionAsync();

        Assert.True(this.context.Database.CurrentTransaction == null);
    }
}