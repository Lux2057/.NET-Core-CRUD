﻿namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.Core;
    using CRUD.DAL;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class ExampleEntity : IId<int>
    {
        #region Properties

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
                CreateMap<ExampleEntity, ExampleDto>()
                        .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                        .ForMember(r => r.Text, r => r.MapFrom(x => x.Text))
                        .ForMember(r => r.Number, r => r.MapFrom(x => x.Number))
                        .ForMember(r => r.Flag, r => r.MapFrom(x => x.Flag))
                        .ForMember(r => r.EnumValue, r => r.MapFrom(x => x.EnumValue));

                CreateMap<ExampleEntity, ExampleTextDto>()
                        .ForMember(r => r.Id, r => r.MapFrom(x => x.Id))
                        .ForMember(r => r.Text, r => r.MapFrom(x => x.Text));
            }

            #endregion
        }

        public class Mapping : IEntityTypeConfiguration<ExampleEntity>
        {
            #region Interface Implementations

            public void Configure(EntityTypeBuilder<ExampleEntity> builder)
            {
                builder.HasKey(r => r.Id);
                builder.Property(r => r.Id).ValueGeneratedOnAdd();
                builder.Property(r => r.Text).HasColumnTypeText();
                builder.Property(r => r.Number).IsRequired();
                builder.Property(r => r.Flag).IsRequired();
                builder.PropertyAsEnum(r => r.EnumValue).IsRequired();
            }

            #endregion
        }

        #endregion

        public int Id { get; set; }
    }
}