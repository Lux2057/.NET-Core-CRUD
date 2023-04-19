namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public class TestEntity : EntityBase<int>
{
    #region Properties

    public string Text { get; set; }

    #endregion

    #region Nested Classes

    public class Mapping : IEntityTypeConfiguration<TestEntity>
    {
        #region Interface Implementations

        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.CrDt);
            builder.Property(r => r.Text);
        }

        #endregion
    }

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