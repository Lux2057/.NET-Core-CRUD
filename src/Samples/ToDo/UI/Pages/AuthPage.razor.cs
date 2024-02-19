namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

[Route(UiRoutes.Auth)]
public partial class AuthPage : PageBase<AuthState>
{
    #region Constants

    private const string signInModalId = "sign-in-modal";

    private const string signUpModalId = "sign-up-modal";

    private const string signInLabelId = "sign-in-label";

    private const string signUpLabelId = "sign-up-label";

    #endregion

    #region Properties

    string UserName { get; set; }

    string Password { get; set; }

    #endregion

    void ClearValidation()
    {
        Dispatcher.Dispatch(new SetValidationStateWf.Init(null));
    }

    void SignIn()
    {
        ClearValidation();
        Dispatcher.Dispatch(new AuthWf.SignInWf.Init(new AuthRequest
                                                     {
                                                             UserName = UserName,
                                                             Password = Password
                                                     },
                                                     authResult => { }));
    }

    void SignUp()
    {
        ClearValidation();
        Dispatcher.Dispatch(new AuthWf.SignUpWf.Init(new AuthRequest
                                                     {
                                                             UserName = UserName,
                                                             Password = Password
                                                     },
                                                     authResult => { }));
    }
}