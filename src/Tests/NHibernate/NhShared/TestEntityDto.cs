namespace NhTests.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;
using JetBrains.Annotations;

#endregion

public class TestEntityDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public string Text { get; set; }

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