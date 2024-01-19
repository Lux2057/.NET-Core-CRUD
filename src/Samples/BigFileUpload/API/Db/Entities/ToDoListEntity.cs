namespace Samples.UploadBigFile.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Samples.UploadBigFile.Shared;

#endregion

[Table("ToDoList")]
public class ToDoListEntity : ApiEntityBase, NameProp.Interface
{
    #region Properties

    public string Name { get; set; }

    public virtual ICollection<ToDoListItemEntity> Items { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    private class Mapping : ApiMappingBase<ToDoListEntity>
    {
        public override void Configure(EntityTypeBuilder<ToDoListEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Name).IsRequired();
        }
    }

    [UsedImplicitly]
    public class Profile : AutoMapper.Profile
    {
        #region Constructors

        public Profile()
        {
            CreateMap<ToDoListEntity, ToDoListDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Name, r => r.MapFrom(x => x.Name))
                    .ForMember(r => r.CrDt, r => r.MapFrom(x => x.CrDt))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}