namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table(TablesNames.Statuses)]
public class StatusEntity : EntityBase,
                            NameProp.Interface,
                            UserProp.Interface
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
            builder.HasOne(r => r.User).WithMany(r => r.Statuses).HasForeignKey(r => r.UserId);
        }
    }

    #endregion
}