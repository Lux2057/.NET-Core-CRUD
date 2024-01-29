namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table(TablesNames.Tags)]
public class TagEntity : EntityBase, NameProp.Interface
{
    #region Properties

    public string Name { get; set; }

    public virtual ICollection<ProjectToTagEntity> Projects { get; set; }

    public virtual ICollection<TaskToTagEntity> Tasks { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<TagEntity>
    {
        public override void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name).IsRequired();
        }
    }

    [UsedImplicitly]
    public class Automap : Profile
    {
        #region Constructors

        protected Automap()
        {
            CreateMap<TagEntity, TagDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Name, r => r.MapFrom(x => x.Name))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}