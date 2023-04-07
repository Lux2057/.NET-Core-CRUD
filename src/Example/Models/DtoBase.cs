namespace CRUD.Example
{
    #region << Using >>

    using CRUD.Core;

    #endregion

    public abstract class DtoBase : IId<int?>
    {
        #region Properties

        public int? Id { get; set; }

        #endregion
    }
}