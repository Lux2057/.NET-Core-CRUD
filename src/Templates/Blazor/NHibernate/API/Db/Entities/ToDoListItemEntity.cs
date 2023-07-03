namespace Templates.Blazor.NH.API;

#region << Using >>

using CRUD.DAL.NHibernate;
using JetBrains.Annotations;
using Templates.Blazor.NH.Shared;

#endregion

public class ToDoListItemEntity : ApiEntityBase,
                                  DescriptionProp.Interface,
                                  StatusProp.Interface,
                                  ToDoListProp.Interface
{
    #region Properties

    public virtual string Description { get; set; }

    public virtual ItemStatus Status { get; set; }

    public virtual ToDoListEntity ToDoList { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : ApiMappingBase<ToDoListItemEntity>
    {
        #region Constructors

        public Mapping()
        {
            Table("ToDoListItemNH");
            Map(r => r.Description).TextSqlType().Not.Nullable();
            Map(r => r.Status).Not.Nullable();
            References(r => r.ToDoList).ForeignKey("ToDoListId");
        }

        #endregion
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
                    .ForMember(r => r.ToDoListId, r => r.MapFrom(x => x.ToDoList.Id))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}