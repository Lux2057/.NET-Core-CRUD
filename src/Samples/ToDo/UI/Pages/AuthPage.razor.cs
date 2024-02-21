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

    #endregion

    #region Properties

    string UserName { get; set; }

    string Password { get; set; }

    #endregion

    void ClearFields()
    {
        Dispatcher.Dispatch(new SetValidationStateWf.Init(null, null));

        UserName = string.Empty;
        Password = string.Empty;
    }

    void SignIn()
    {
        Dispatcher.Dispatch(new SignInWf.Init(new AuthRequest
                                              {
                                                      UserName = UserName,
                                                      Password = Password
                                              },
                                              async authInfo =>
                                              {
                                                  if (authInfo == null)
                                                      return;

                                                  await JS.CloseModal(signInModalId);

                                                  Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Projects, false));
                                              }));
    }

    void SignUp()
    {
        Dispatcher.Dispatch(new SignUpWf.Init(new AuthRequest
                                              {
                                                      UserName = UserName,
                                                      Password = Password
                                              },
                                              async authInfo =>
                                              {
                                                  if (authInfo == null)
                                                      return;

                                                  await JS.CloseModal(signUpModalId);

                                                  Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Projects, false));
                                              }));
    }
}