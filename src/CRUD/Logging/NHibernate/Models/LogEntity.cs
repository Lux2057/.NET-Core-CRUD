﻿namespace CRUD.Logging.NHibernate;

#region << Using >>

using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

#endregion

public class LogEntity : IId<int>
{
    #region Properties

    public virtual int Id { get; set; }

    public virtual DateTime CrDt { get; set; }

    public virtual LogLevel LogLevel { get; set; }

    public virtual string Message { get; set; }

    public virtual string Exception { get; set; }

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