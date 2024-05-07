namespace Samples.ToDo.API;

#region << Using >>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public abstract class MappingBase<T> : IEntityTypeConfiguration<T> where T : EntityBase, new()
{
    #region Interface Implementations

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.CrDt).IsRequired();
        builder.Property(r => r.IsDeleted).IsRequired().HasDefaultValue(false);
    }

    #endregion
}