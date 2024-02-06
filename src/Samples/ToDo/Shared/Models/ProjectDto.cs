namespace Samples.ToDo.Shared;

public class ProjectDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TagDto[] Tags { get; set; }

    #endregion

    #region Nested Classes

    public class GetRequest
    {
        public string SearchTerm { get; set; }

        public int[] TagsIds { get; set; }
    }

    public class CreateRequest
    {
        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public int[] TagsIds { get; set; }

        #endregion
    }

    public class EditRequest
    {
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int[] TagsIds { get; set; }

        #endregion
    }

    #endregion
}