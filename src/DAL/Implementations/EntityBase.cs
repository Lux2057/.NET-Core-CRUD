namespace CRUD.DAL
{
    #region << Using >>

    using System;

    #endregion

    public abstract class EntityBase
    {
        #region Properties

        public virtual object Id { get; set; }

        public DateTime CrDt { get; set; }

        #endregion
    }
}