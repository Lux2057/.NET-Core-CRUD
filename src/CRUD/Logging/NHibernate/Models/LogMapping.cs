namespace CRUD.Logging.NHibernate;

#region << Using >>

using CRUD.DAL.NHibernate;
using FluentNHibernate.Mapping;

#endregion

public class LogMapping : ClassMap<LogEntity>
{
    #region Constructors

    public LogMapping()
    {
        Table("Logs");
        Id(r => r.Id).GeneratedId();
        Map(x => x.CrDt).Not.Nullable();
        Map(x => x.LogLevel).Not.Nullable();
        Map(x => x.Message).Not.Nullable().CustomSqlType("text");
        Map(x => x.Exception).Nullable().CustomSqlType("text");
    }

    #endregion
}