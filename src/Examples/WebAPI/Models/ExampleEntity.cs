namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.DAL.Abstractions;
    using CRUD.DAL.EntityFramework;
    using CRUD.DAL.NHibernate;
    using FluentNHibernate.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    public class ExampleEntity : IId<int>
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual string Text { get; set; }

        public virtual int Number { get; set; }

        public virtual bool Flag { get; set; }

        public virtual ExampleEnum EnumValue { get; set; }

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

        public class EfMapping : IEntityTypeConfiguration<ExampleEntity>
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

        public class NhMapping : ClassMap<ExampleEntity>
        {
            #region Constructors

            public NhMapping()
            {
                Id(r => r.Id).GeneratedId();
                Map(x => x.Text).TextSqlType().Nullable();
                Map(x => x.Number).Not.Nullable();
                Map(x => x.Flag).Not.Nullable();
                Map(x => x.EnumValue).Not.Nullable();
            }

            #endregion
        }

        #endregion
    }
}