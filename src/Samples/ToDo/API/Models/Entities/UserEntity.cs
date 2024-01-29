namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table(TablesNames.Users)]
public class UserEntity : EntityBase
{
    #region Properties

    public string Login { get; set; }

    public string PasswordHash { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; }

    public virtual ICollection<ProjectToTagEntity> ProjectsToTags { get; set; }

    public virtual ICollection<TaskEntity> Tasks { get; set; }

    public virtual ICollection<StatusEntity> Statuses { get; set; }

    public virtual ICollection<TaskToTagEntity> TasksToTags { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Login).IsRequired();
            builder.Property(r => r.PasswordHash).IsRequired();
        }
    }

    [UsedImplicitly]
    public class Automap : Profile
    {
        #region Constructors

        protected Automap()
        {
            CreateMap<UserEntity, UserDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Login, r => r.MapFrom(x => x.Login))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}