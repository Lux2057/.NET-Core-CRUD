namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.Core;

    #endregion

    public class ExampleDto : IId<int>
    {
        #region Properties

        public int Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Flag { get; set; }

        public ExampleEnum EnumValue { get; set; }

        #endregion

        #region Nested Classes

        public class Profile : AutoMapper.Profile
        {
            #region Constructors

            public Profile()
            {
                CreateMap<ExampleDto, ExampleEntity>()
                        .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                        .ForMember(r => r.Text, r => r.MapFrom(x => x.Text))
                        .ForMember(r => r.Number, r => r.MapFrom(x => x.Number))
                        .ForMember(r => r.Flag, r => r.MapFrom(x => x.Flag))
                        .ForMember(r => r.EnumValue, r => r.MapFrom(x => x.EnumValue));
            }

            #endregion
        }

        #endregion
    }
}