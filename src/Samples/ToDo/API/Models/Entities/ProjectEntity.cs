namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using CRUD.DAL.EntityFramework;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Samples.ToDo.Shared;

#endregion

[Table(TablesNames.Projects)]
public class ProjectEntity : EntityBase,
                             NameProp.Interface,
                             DescriptionProp.Interface,
                             UserProp.Interface,
                             IUpDt
{
    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public DateTime? UpDt { get; set; }

    public virtual ICollection<ProjectToTagEntity> Tags { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<ProjectEntity>
    {
        public override void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Description).HasColumnTypeText();
            builder.Property(r => r.UpDt);
            builder.HasOne(r => r.User).WithMany(r => r.Projects).HasForeignKey(r => r.UserId);
        }
    }

    [UsedImplicitly]
    public class Automap : Profile
    {
        #region Constructors

        protected Automap()
        {
            CreateMap<ProjectEntity, ProjectDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Name, r => r.MapFrom(x => x.Name))
                    .ForMember(r => r.Description, r => r.MapFrom(x => x.Description))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}