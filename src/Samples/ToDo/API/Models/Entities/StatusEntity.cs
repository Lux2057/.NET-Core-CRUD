namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Samples.ToDo.Shared;

#endregion

[Table(TablesNames.Statuses)]
public class StatusEntity : EntityBase,
                            NameProp.Interface,
                            UserIdProp.Interface
{
    #region Properties

    public string Name { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public virtual ICollection<TaskEntity> Tasks { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<StatusEntity>
    {
        public override void Configure(EntityTypeBuilder<StatusEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Name).IsRequired();
            builder.HasOne(r => r.User).WithMany(r => r.Statuses).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    [UsedImplicitly]
    public class Automap : Profile
    {
        #region Constructors

        public Automap()
        {
            CreateMap<StatusEntity, StatusDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Name, r => r.MapFrom(x => x.Name))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}