namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.Core;
    using CRUD.DAL;

    #endregion

    public class ExampleTextDto : IId<int?>
    {
        #region Properties

        public int? Id { get; set; }

        public string Text { get; set; }

        #endregion
    }
}