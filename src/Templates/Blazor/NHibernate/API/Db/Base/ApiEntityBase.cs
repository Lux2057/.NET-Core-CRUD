namespace Templates.Blazor.NH.API
{
    #region << Using >>

    using CRUD.DAL.Abstractions;
    using Templates.Blazor.NH.Shared;

    #endregion

    public abstract class ApiEntityBase : IId<int>, IDt
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual DateTime CrDt { get; set; }

        public virtual DateTime? UpDt { get; set; }

        #endregion
    }
}