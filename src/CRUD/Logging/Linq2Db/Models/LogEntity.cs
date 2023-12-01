namespace CRUD.Logging.Linq2Db
{
    #region << Using >>

    using CRUD.DAL.Abstractions;
    using JetBrains.Annotations;
    using LinqToDB;
    using LinqToDB.Mapping;
    using Microsoft.Extensions.Logging;

    #endregion

    [Table("Logs")]
    public class LogEntity : IId<string>
    {
        #region Properties

        [PrimaryKey]
        public string Id { get; set; }

        [Column]
        public DateTime CrDt { get; set; }

        [Column]
        public LogLevel LogLevel { get; set; }

        [Column, DataType(DataType.Text)]
        public string Message { get; set; }

        [Column, DataType(DataType.Text)]
        public string Exception { get; set; }

        #endregion

        #region Constructors

        public LogEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region Nested Classes

        [UsedImplicitly]
        public class Profile : AutoMapper.Profile
        {
            #region Constructors

            public Profile()
            {
                CreateMap<LogEntity, LogDto>()
                        .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                        .ForMember(r => r.CrDt, r => r.MapFrom(x => x.CrDt))
                        .ForMember(r => r.LogLevel, r => r.MapFrom(x => x.LogLevel.ToString()))
                        .ForMember(r => r.Message, r => r.MapFrom(x => x.Message))
                        .ForMember(r => r.Exception, r => r.MapFrom(x => x.Exception));
            }

            #endregion
        }

        #endregion
    }
}