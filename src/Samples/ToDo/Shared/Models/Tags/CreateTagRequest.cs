﻿namespace Samples.ToDo.Shared;

public interface ICreateTagRequest
{
    #region Properties

    public string Name { get; }

    #endregion
}

public class CreateTagRequest : ICreateTagRequest
{
    #region Properties

    public string Name { get; set; }

    #endregion
}