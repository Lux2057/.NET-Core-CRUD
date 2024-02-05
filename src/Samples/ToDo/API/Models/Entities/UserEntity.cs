namespace Samples.ToDo.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using AutoMapper;
using Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

[Table(TablesNames.Users)]
public class UserEntity : EntityBase
{
    #region Properties

    public string UserName { get; set; }

    public string PasswordHash { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; }

    public virtual ICollection<ProjectToTagEntity> ProjectsToTags { get; set; }

    public virtual ICollection<TaskEntity> Tasks { get; set; }

    public virtual ICollection<StatusEntity> Statuses { get; set; }

    public virtual ICollection<TaskToTagEntity> TasksToTags { get; set; }

    public virtual ICollection<RefreshTokenEntity> Tokens { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : MappingBase<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.UserName).IsRequired();
            builder.Property(r => r.PasswordHash).IsRequired();
        }
    }

    [UsedImplicitly]
    public class Automap : Profile
    {
        #region Constructors

        public Automap()
        {
            CreateMap<UserEntity, UserDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.UserName, r => r.MapFrom(x => x.UserName))
                    .ReverseMap();
        }

        #endregion
    }

    public abstract class FindBy
    {
        #region Nested Classes

        public class UserNameEqualTo : SpecificationBase<UserEntity>
        {
            #region Properties

            private readonly bool caseSensitive;

            private readonly string userName;

            #endregion

            #region Constructors

            public UserNameEqualTo(string userName, bool caseSensitive = true)
            {
                this.userName = userName;
                this.caseSensitive = caseSensitive;
            }

            #endregion

            public override Expression<Func<UserEntity, bool>> ToExpression()
            {
                if (this.userName.IsNullOrWhitespace())
                    return x => true;

                if (this.caseSensitive)
                    return x => x.UserName == this.userName;

                return x => x.UserName.ToLower() == this.userName.ToLower();
            }
        }

        #endregion
    }

    #endregion
}