namespace Templates.Blazor.EF.API
{
    #region << Using >>

    using CRUD.DAL.Abstractions;
    using Templates.Blazor.EF.Shared;

    #endregion

    public abstract class ApiEntityBase : IId<int>, IDt
    {
        #region Properties

        public int Id { get; set; }

        public DateTime CrDt { get; set; }

        public DateTime? UpDt { get; set; }

        #endregion
    }
}