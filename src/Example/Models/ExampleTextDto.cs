namespace CRUD.Example
{
    #region << Using >>

    using CRUD.Core;

    #endregion

    public class ExampleTextDto : IId<int?>
    {
        #region Properties

        public new int? Id { get; set; }

        public string Text { get; set; }

        #endregion
    }
}