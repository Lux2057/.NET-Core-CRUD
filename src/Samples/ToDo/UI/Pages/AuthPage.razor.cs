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

    private AuthRequest authRequest { get; set; } = new();

    #endregion

    private void clearFields()
    {
        Dispatcher.Dispatch(new SetValidationStateWf.Init(null, null));

        authRequest = new AuthRequest();
    }

    private void signIn()
    {
        if (State.IsLoading)
            return;

        Dispatcher.Dispatch(new SignInWf.Init(request: authRequest,
                                              callback: async authInfo =>
                                                        {
                                                            if (authInfo == null)
                                                                return;

                                                            await JS.CloseModalAsync(signInModalId);

                                                            Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Projects));
                                                        },
                                              validationKey: signInValidationKey));
    }

    private void signUp()
    {
        if (State.IsLoading)
            return;

        Dispatcher.Dispatch(new SignUpWf.Init(request: authRequest,
                                              callback: async authInfo =>
                                                        {
                                                            if (authInfo == null)
                                                                return;

                                                            await JS.CloseModalAsync(signUpModalId);

                                                            Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Projects));
                                                        },
                                              validationKey: signUpValidationKey));
    }
}