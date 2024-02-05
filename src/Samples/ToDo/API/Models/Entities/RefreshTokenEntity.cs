namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table(TablesNames.RefreshTokens)]
public class RefreshTokenEntity : EntityBase,
                                  UserIdProp.Interface
{
    #region Properties

    public string TokenHash { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<RefreshTokenEntity>
    {
        public override void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.TokenHash).IsRequired();
            builder.Property(r => r.IssuedAt).IsRequired();
            builder.Property(r => r.ExpiresAt).IsRequired();
            builder.HasOne(r => r.User).WithMany(r => r.Tokens).HasForeignKey(r => r.UserId);
        }
    }

    public abstract class FindBy
    {
        #region Nested Classes

        public class ExpiresAtGreaterOrEqualTo : SpecificationBase<RefreshTokenEntity>
        {
            #region Properties

            private readonly DateTime expiresAt;

            #endregion

            #region Constructors

            public ExpiresAtGreaterOrEqualTo(DateTime expiresAt)
            {
                this.expiresAt = expiresAt;
            }

            #endregion

            public override Expression<Func<RefreshTokenEntity, bool>> ToExpression()
            {
                return x => x.ExpiresAt >= this.expiresAt;
            }
        }

        #endregion
    }

    #endregion
}