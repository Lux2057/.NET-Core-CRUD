namespace Templates.Blazor.NH.API;

#region << Using >>

using JetBrains.Annotations;
using Templates.Blazor.NH.Shared;

#endregion

public class ToDoListEntity : ApiEntityBase, NameProp.Interface
{
    #region Properties

    public virtual string Name { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Mapping : ApiMappingBase<ToDoListEntity>
    {
        #region Constructors

        public Mapping()
        {
            Table("ToDoListNH");
            Map(r => r.Name).Not.Nullable();
        }

        #endregion
    }

    [UsedImplicitly]
    public class Profile : AutoMapper.Profile
    {
        #region Constructors

        public Profile()
        {
            CreateMap<ToDoListEntity, ToDoListDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Name, r => r.MapFrom(x => x.Name))
                    .ForMember(r => r.CrDt, r => r.MapFrom(x => x.CrDt))
                    .ReverseMap();
        }

        #endregion
    }

    #endregion
}