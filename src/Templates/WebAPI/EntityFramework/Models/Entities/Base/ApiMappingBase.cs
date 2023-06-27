namespace WebAPI_Template_EF;

#region << Using >>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public abstract class ApiMappingBase<T> : IEntityTypeConfiguration<T> where T : ApiEntityBase, new()
{
    #region Interface Implementations

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.CrDt).IsRequired();
        builder.Property(r => r.UpDt);
    }

    #endregion
}