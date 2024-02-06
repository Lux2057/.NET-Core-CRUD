namespace Samples.ToDo.API;

public class StatusDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    #endregion

    #region Nested Classes

    public class EditRequest
    {
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        #endregion
    }

    public class CreateRequest
    {
        #region Properties

        public string Name { get; set; }

        #endregion
    }

    #endregion
}