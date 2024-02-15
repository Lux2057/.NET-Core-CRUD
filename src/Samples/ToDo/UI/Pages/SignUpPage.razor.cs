namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

[Route(UiRoutes.SignUp)]
public partial class SignUpPage : PageBase
{
    #region Properties

    private string UserName { get; set; }

    string Password { get; set; }

    #endregion

    void SignUp()
    {
        Dispatcher.Dispatch(new AuthWf.SignUp.Init(new AuthRequest
                                                   {
                                                           UserName = UserName,
                                                           Password = Password
                                                   },
                                                   authResult => { }));
    }
}