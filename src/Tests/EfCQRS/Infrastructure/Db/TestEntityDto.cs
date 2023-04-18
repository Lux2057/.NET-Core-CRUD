namespace EfTests.CQRS;

public class TestEntityDto
{
    #region Properties

    public int Id { get; init; }

    public string Text { get; init; }

    #endregion

    #region Nested Classes

    public class Profile : AutoMapper.Profile
    {
        #region Constructors

        public Profile()
        {
            CreateMap<TestEntityDto, TestEntity>();
        }

        #endregion
    }

    #endregion
}