namespace CRUD.DAL.NHibernate;

#region << Using >>

using FluentNHibernate.Mapping;

#endregion

public static class NhMappingExt
{
    public static PropertyPart TextSqlType(this PropertyPart propertyPart)
    {
        return propertyPart.CustomSqlType("text");
    }

    public static IdentityPart GeneratedId(this IdentityPart identityPart)
    {
        return identityPart.GeneratedBy.Increment();
    }
}