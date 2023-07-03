namespace Templates.Blazor.NH.API
{
    #region << Using >>

    using CRUD.DAL.NHibernate;
    using FluentNHibernate.Mapping;

    #endregion

    public abstract class ApiMappingBase<T> : ClassMap<T> where T : ApiEntityBase, new()
    {
        #region Constructors

        public ApiMappingBase()
        {
            Id(r => r.Id).GeneratedId();
            Map(r => r.CrDt);
            Map(r => r.UpDt);
        }

        #endregion
    }
}