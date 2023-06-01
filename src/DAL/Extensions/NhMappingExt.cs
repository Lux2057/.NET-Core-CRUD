﻿namespace CRUD.DAL;

#region << Using >>

using FluentNHibernate.Mapping;

#endregion

public static class NhMappingExt
{
    public static PropertyPart TextSqlType(this PropertyPart propertyPart)
    {
        return propertyPart.CustomSqlType("text");
    }
}