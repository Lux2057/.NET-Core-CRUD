namespace CRUD.Logging.NHibernate;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using CRUD.DAL.NHibernate;
using CRUD.Logging.Common;
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