namespace Linq2DbTests.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using LinqToDB.Mapping;

#endregion

public class TestEntity : IId<string>
{
    #region Properties

    [PrimaryKey]
    public string Id { get; set; }

    [Column, LinqToDB.Mapping.NotNull]
    public string Text { get; set; }

    #endregion

    #region Constructors

    public TestEntity()
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
            CreateMap<TestEntity, TestEntityDto>();
        }

        #endregion
    }

    #endregion
}