namespace EfTests.CQRS;

#region << Using >>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public class TestEntity
{
    #region Properties

    public int Id { get; set; }

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