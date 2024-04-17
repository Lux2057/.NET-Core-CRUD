namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using CRUD.DAL.EntityFramework;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Samples.ToDo.Shared;

#endregion

[Table(TablesNames.Tasks)]
public class TaskEntity : EntityBase,
                          NameProp.Interface,
                          DescriptionProp.Interface,
                          UserIdProp.Interface,
                          ProjectIdProp.Interface,
                          IUpDt
{
    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? UpDt { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public TaskStatus Status { get; set; }

    public int ProjectId { get; set; }

    public virtual ProjectEntity Project { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<TaskEntity>
    {
        public override void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Description).HasColumnTypeText();
            builder.Property(r => r.UpDt);
            builder.Property(r => r.Status).IsRequired();
            builder.HasOne(r => r.Project).WithMany(r => r.Tasks).HasForeignKey(r => r.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(r => r.User).WithMany(r => r.Tasks).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [UsedImplicitly]
    public class Automap : Profile
    {
        #region Constructors

        public Automap()
        {
            CreateMap<TaskEntity, TaskDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Name, r => r.MapFrom(x => x.Name))
                    .ForMember(r => r.Description, r => r.MapFrom(x => x.Description))
                    .ForMember(r => r.Status, r => r.MapFrom(x => x.Status))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}