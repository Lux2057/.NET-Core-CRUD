namespace CRUD.DAL.LINQ2DB;

#region << Using >>

using LinqToDB;
using LinqToDB.Data;

#endregion

public static class DataConnectionExt
{
    public static void TryCreateTable<TEntity>(this DataConnection connection, string tableName) where TEntity : class, new()
    {
        if (connection.DataProvider.GetSchemaProvider().GetSchema(connection).Tables.Any(r => r.TableName == tableName))
            return;

        connection.CreateTable<TEntity>(tableName);
    }
}