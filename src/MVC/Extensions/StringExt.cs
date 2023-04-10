namespace CRUD.MVC
{
    #region << Using >>

    using System;

    #endregion

    public static class StringExt
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}