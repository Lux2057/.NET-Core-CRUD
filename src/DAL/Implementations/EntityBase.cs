namespace CRUD.DAL
{
    #region << Using >>

    using System;

    #endregion

    public abstract class EntityBase
    {
        #region Properties

        public int Id { get; set; }

        public DateTime CrDt { get; set; }

        #endregion
    }
}