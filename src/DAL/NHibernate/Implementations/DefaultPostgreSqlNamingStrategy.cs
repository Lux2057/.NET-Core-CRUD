namespace CRUD.DAL;

#region << Using >>

using NHibernate.Cfg;

#endregion

public class DefaultPostgreSqlNamingStrategy : INamingStrategy
{
    #region Interface Implementations

    public string ClassToTableName(string className)
    {
        return DoubleQuote(className);
    }

    public string PropertyToColumnName(string propertyName)
    {
        return DoubleQuote(propertyName);
    }

    public string TableName(string tableName)
    {
        return DoubleQuote(tableName);
    }

    public string ColumnName(string columnName)
    {
        return DoubleQuote(columnName);
    }

    public string PropertyToTableName(string className,
                                      string propertyName)
    {
        return DoubleQuote(propertyName);
    }

    public string LogicalColumnName(string columnName,
                                    string propertyName)
    {
        return string.IsNullOrWhiteSpace(columnName) ?
                       DoubleQuote(propertyName) :
                       DoubleQuote(columnName);
    }

    #endregion

    private static string DoubleQuote(string raw)
    {
        raw = raw.Replace("`", "");
        return $"\"{raw}\"";
    }
}