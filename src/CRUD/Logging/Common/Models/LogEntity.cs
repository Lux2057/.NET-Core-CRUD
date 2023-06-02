namespace CRUD.Logging.Common
{
    #region << Using >>

    using System.ComponentModel.DataAnnotations.Schema;
    using CRUD.DAL.Abstractions;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;

    #endregion

    [Table("Logs")]
    public class LogEntity : IId<int>
    {
        #region Properties

        public int Id { get; set; }

        public DateTime CrDt { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

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