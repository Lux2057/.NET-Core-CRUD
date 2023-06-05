namespace EfTests.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class TestEntityDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public DateTime CrDt { get; set; }

    public string Text { get; set; }

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