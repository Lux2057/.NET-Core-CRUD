namespace CRUD.Example
{
    #region << Using >>

    using CRUD.MVC;

    #endregion

    public class ExampleTextDto : DtoBase
    {
        #region Properties

        public new int? Id { get; set; }

        public string Text { get; set; }

        #endregion
    }
}