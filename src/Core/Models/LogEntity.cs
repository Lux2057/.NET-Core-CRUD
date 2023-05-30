namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using CRUD.DAL;
    using CRUD.Extensions;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

                CreateMap<LogDto, LogEntity>()
                        .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                        .ForMember(r => r.LogLevel, r => r.MapFrom(x => x.LogLevel.ToEnum<LogLevel>()))
                        .ForMember(r => r.Message, r => r.MapFrom(x => x.Message))
                        .ForMember(r => r.Exception, r => r.MapFrom(x => x.Exception));
            }

            #endregion
        }

        class LabMapping : IEntityTypeConfiguration<LogEntity>
        {
            #region Interface Implementations

            public void Configure(EntityTypeBuilder<LogEntity> builder)
            {
                builder.HasKey(r => r.Id);
                builder.Property(r => r.Id).ValueGeneratedOnAdd();
                builder.Property(r => r.CrDt).IsRequired();
                builder.PropertyAsEnum(r => r.LogLevel).IsRequired();
                builder.Property(r => r.Message).IsRequired().HasColumnTypeText();
                builder.Property(r => r.Exception).HasColumnTypeText();
            }

            #endregion
        }

        #endregion
    }
}