namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table($"{TablesNames.Projects}_{TablesNames.Tags}")]
public class ProjectToTagEntity : EntityBase,
                                  UserIdProp.Interface,
                                  ProjectIdProp.Interface,
                                  TagIdProp.Interface
{
    #region Properties

    public int ProjectId { get; set; }

    public virtual ProjectEntity Project { get; set; }

    public int TagId { get; set; }

    public virtual TagEntity Tag { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<ProjectToTagEntity>
    {
        public override void Configure(EntityTypeBuilder<ProjectToTagEntity> builder)
        {
            base.Configure(builder);
            builder.HasOne(r => r.Project).WithMany(r => r.Tags).HasForeignKey(r => r.ProjectId);
            builder.HasOne(r => r.Tag).WithMany(r => r.Projects).HasForeignKey(r => r.TagId);
            builder.HasOne(r => r.User).WithMany(r => r.ProjectsToTags).HasForeignKey(r => r.UserId);
        }
    }

    #endregion
}