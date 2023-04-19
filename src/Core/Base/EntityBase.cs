namespace CRUD.Core
{
    #region << Using >>

    using System;

    #endregion

    public abstract class EntityBase<TId> : IId<TId>
    {
        #region Properties

        public TId Id { get; set; }

        public DateTime CrDt { get; set; }

        #endregion

        #region Constructors

        protected EntityBase()
        {
            CrDt = DateTime.UtcNow;
        }

        #endregion
    }
}