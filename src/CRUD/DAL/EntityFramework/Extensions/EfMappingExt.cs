namespace CRUD.DAL.EntityFramework;

#region << Using >>

using System.Linq.Expressions;
using CRUD.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

public static class EfMappingExt
{
    public static PropertyBuilder<TEnum> PropertyAsEnum<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum>> propertyExpression) where TEntity : class, new() where TEnum : struct, Enum
    {
        return builder.Property(propertyExpression)
                      .HasMaxLength(50)
                      .HasConversion(r => r.ToString(),
                                     r => r.ToEnum<TEnum>())
                      .IsUnicode(false);
    }

    public static PropertyBuilder<string> HasColumnTypeText(this PropertyBuilder<string> builder)
    {
        return builder.HasColumnType("text");
    }
}