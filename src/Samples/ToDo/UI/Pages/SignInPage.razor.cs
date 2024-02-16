namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

[Route(UiRoutes.SignIn)]
public partial class SignInPage : PageBase
{
    #region Properties

    string UserName { get; set; }

    string Password { get; set; }

    #endregion

    void SignIn()
    {
        Dispatcher.Dispatch(new AuthWf.SignIn.Init(new AuthRequest
                                                   {
                                                           UserName = UserName,
                                                           Password = Password
                                                   },
                                                   authResult => { }));
    }
}