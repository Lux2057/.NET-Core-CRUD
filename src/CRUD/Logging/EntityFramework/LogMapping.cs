namespace CRUD.Logging.EntityFramework;

#region << Using >>

using CRUD.DAL.EntityFramework;
using CRUD.Logging.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public class LogMapping : IEntityTypeConfiguration<LogEntity>
{
    #region Interface Implementations

    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.CrDt).IsRequired();
        builder.PropertyAsEnum(r => r.LogLevel).IsRequired();
        builder.Property(r => r.Message).IsRequired().HasColumnTypeText();
        builder.Property(r => r.Exception).HasColumnTypeText();
    }

    #endregion
}