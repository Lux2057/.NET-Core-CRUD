namespace WebAPI_Template_EF;

#region << Using >>

using CRUD.DAL.EntityFramework;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public class SampleEntity : ApiEntityBase
{
    #region Properties

    public string Text { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : ApiMappingBase<SampleEntity>
    {
        public override void Configure(EntityTypeBuilder<SampleEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Text).HasColumnTypeText();
        }
    }

    [UsedImplicitly]
    public class Profile : AutoMapper.Profile
    {
        #region Constructors

        public Profile()
        {
            CreateMap<SampleEntity, SampleDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Text, r => r.MapFrom(x => x.Text))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}