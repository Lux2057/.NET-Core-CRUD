namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table($"{TablesNames.Tasks}_{TablesNames.Tags}")]
public class TaskToTagEntity : EntityBase,
                               UserIdProp.Interface,
                               TaskIdProp.Interface,
                               TagIdProp.Interface
{
    #region Properties

    public int TaskId { get; set; }

    public virtual TaskEntity Task { get; set; }

    public int TagId { get; set; }

    public virtual TagEntity Tag { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<TaskToTagEntity>
    {
        public override void Configure(EntityTypeBuilder<TaskToTagEntity> builder)
        {
            base.Configure(builder);
            builder.HasOne(r => r.Task).WithMany(r => r.Tags).HasForeignKey(r => r.TaskId);
            builder.HasOne(r => r.Tag).WithMany(r => r.Tasks).HasForeignKey(r => r.TagId);
            builder.HasOne(r => r.User).WithMany(r => r.TasksToTags).HasForeignKey(r => r.UserId);
        }
    }

    #endregion
}