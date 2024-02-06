﻿namespace Samples.ToDo.API;

public class TagDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    #endregion

    #region Nested Classes

    public class CreateRequest
    {
        #region Properties

        public string Name { get; set; }

        #endregion
    }

    #endregion
}