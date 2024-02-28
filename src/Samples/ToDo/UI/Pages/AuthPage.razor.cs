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

    private const string signInValidationKey = "sign-in-validation";

    private const string signUpValidationKey = "sign-up-validation";

    #endregion

    #region Properties

    AuthRequest AuthRequest { get; set; } = new AuthRequest();

    #endregion

    void ClearFields()
    {
        Dispatcher.Dispatch(new SetValidationStateWf.Init(null, null));

        AuthRequest = new AuthRequest();
    }

    void SignIn()
    {
        if (State.IsLoading)
            return;

        Dispatcher.Dispatch(new SignInWf.Init(request: AuthRequest,
                                              callback: async authInfo =>
                                                        {
                                                            if (authInfo == null)
                                                                return;

                                                            await JS.CloseModalAsync(signInModalId);

                                                            Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Projects, false));
                                                        },
                                              validationKey: signInValidationKey));
    }

    void SignUp()
    {
        if (State.IsLoading)
            return;

        Dispatcher.Dispatch(new SignUpWf.Init(request: AuthRequest,
                                              callback: async authInfo =>
                                                        {
                                                            if (authInfo == null)
                                                                return;

                                                            await JS.CloseModalAsync(signUpModalId);

                                                            Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Projects, false));
                                                        },
                                              validationKey: signUpValidationKey));
    }
}