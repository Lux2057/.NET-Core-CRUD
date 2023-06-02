namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.DAL.NHibernate;
using FluentNHibernate.Mapping;
using JetBrains.Annotations;

#endregion

public class TestEntity : IId<int>
{
    #region Properties

    public virtual int Id { get; set; }

    public virtual string Text { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Mapping : ClassMap<TestEntity>
    {
        #region Constructors

        public Mapping()
        {
            Id(r => r.Id).GeneratedId();
            Map(r => r.Text);
        }

        #endregion
    }

    #endregion
}