namespace Linq2DbTests.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;
using JetBrains.Annotations;

#endregion

public class TestEntityDto : IId<string>
{
    #region Properties

    public string Id { get; set; }

    public string Text { get; set; }

    #endregion

    #region Constructors

    public TestEntityDto()
    {
        Id = Guid.NewGuid().ToString();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
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