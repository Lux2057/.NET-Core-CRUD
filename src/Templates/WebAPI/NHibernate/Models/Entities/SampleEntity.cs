namespace Templates.WebAPI.NH;

#region << Using >>

using CRUD.DAL.NHibernate;
using JetBrains.Annotations;

#endregion

public class SampleEntity : ApiEntityBase
{
    #region Properties

    public virtual string Text { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Profile : AutoMapper.Profile
    {
        #region Constructors

        public Profile()
        {
            CreateMap<SampleEntity, SampleDto>()
                    .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                    .ForMember(r => r.Text, r => r.MapFrom(x => x.Text))
                    .ReverseMap();
        }

        #endregion
    }

    [UsedImplicitly]
    class Mapping : ApiMappingBase<SampleEntity>
    {
        #region Constructors

        public Mapping()
        {
            Map(r => r.Text).TextSqlType();
        }

        #endregion
    }

    #endregion
}