namespace CRUD.DAL.Linq2Db;

#region << Using >>

using LinqToDB;
using LinqToDB.Data;

#endregion

public static class DataConnectionExt
{
    public static void TryCreateTable<TEntity>(this DataConnection connection,
                                               string tableName,
                                               bool dropIfExists = false) where TEntity : class, new()
    {
        if (connection.DataProvider.GetSchemaProvider().GetSchema(connection).Tables.Any(r => r.TableName == tableName))
        {
            if (dropIfExists)
                connection.DropTable<TEntity>(tableName);
            else
                return;
        }

        connection.CreateTable<TEntity>(tableName);
    }
}