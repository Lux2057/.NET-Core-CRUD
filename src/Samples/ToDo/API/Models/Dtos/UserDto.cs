namespace Samples.ToDo.API;

#region << Using >>

using FluentValidation;
using JetBrains.Annotations;

#endregion

public class UserDto
{
    #region Properties

    public int Id { get; set; }

    public string UserName { get; set; }

    #endregion
}