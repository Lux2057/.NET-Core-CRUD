namespace Templates.Blazor.EF.API;

#region << Using >>

using System.ComponentModel.DataAnnotations.Schema;
using CRUD.DAL.EntityFramework;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templates.Blazor.EF.Shared;

#endregion

[Table("ToDoListItem")]
public class ToDoListItemEntity : ApiEntityBase, 
                                  DescriptionProp.Interface,
                                  StatusProp.Interface,
                                  ToDoListIdProp.Interface
{
    #region Properties

    public string Description { get; set; }

    public ItemStatus Status { get; set; }

    public int ToDoListId { get; set; }

    public virtual ToDoListEntity ToDoList { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : ApiMappingBase<ToDoListItemEntity>
    {
        public override void Configure(EntityTypeBuilder<ToDoListItemEntity> builder)
        {
            base.Configure(builder);
            builder.Property(r => r.Description).HasColumnTypeText().IsRequired();
            builder.PropertyAsEnum(r => r.Status).IsRequired();
            builder.HasOne(r => r.ToDoList).WithMany(r => r.Items).HasForeignKey(r => r.ToDoListId);
        }
    }

    [UsedImplicitly]
    public class Profile : AutoMapper.Profile
    {
        #region Constructors

        public Profile()
        {
            CreateMap<ToDoListItemEntity, ToDoListItemDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Status, r => r.MapFrom(x => x.Status))
                    .ForMember(r => r.Description, r => r.MapFrom(x => x.Description))
                    .ForMember(r => r.ToDoListId, r => r.MapFrom(x => x.ToDoListId))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}