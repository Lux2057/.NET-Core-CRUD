namespace Samples.ToDo.Shared;

public class TaskDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int StatusId { get; set; }

    public TagDto[] Tags { get; set; }

    #endregion

    #region Nested Classes

    public class CreateRequest
    {
        #region Properties

        public string Name { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int[] TagsIds { get; set; }

        #endregion
    }

    public class EditRequest
    {
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int[] TagsIds { get; set; }

        #endregion
    }

    #endregion
}