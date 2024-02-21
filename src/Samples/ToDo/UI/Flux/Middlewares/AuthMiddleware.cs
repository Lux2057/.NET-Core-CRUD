namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

[UsedImplicitly]
public class AuthMiddleware : Middleware
{
    #region Properties

    private IDispatcher Dispatcher;

    private IStore Store;

    #endregion

    public override async Task InitializeAsync(IDispatcher dispatcher, IStore store)
    {
        await base.InitializeAsync(dispatcher, store);

        this.Dispatcher = dispatcher;
        this.Store = store;

        this.Store.UnhandledException += (_, exceptionArgs) =>
                                         {
                                             if (exceptionArgs.Exception.GetType() == typeof(UnauthorizedAccessException))
                                                 this.Dispatcher.Dispatch(new SignOutWf.Init(() => this.Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Auth, true))));
                                         };
    }

    public override bool MayDispatchAction(object action)
    {
        if (action is IAuthenticatedAction authenticatedAction)
        {
            var authState = AuthState();

            if (authState?.IsAuthenticated != true)
            {
                this.Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Auth, true));

                return false;
            }

            if (authState.IsExpiring)
            {
                this.Dispatcher.Dispatch(new RefreshAccessTokenWf.Init(authState.AuthInfo.RefreshToken,
                                                                       authInfo =>
                                                                       {
                                                                           if (authInfo == null)
                                                                               throw new UnauthorizedAccessException();

                                                                           authenticatedAction.AccessToken = authInfo.AccessToken;

                                                                           this.Dispatcher.Dispatch(action);
                                                                       }));

                return false;
            }

            authenticatedAction.AccessToken = authState.AuthInfo.AccessToken;
        }

        return base.MayDispatchAction(action);
    }

    AuthState AuthState()
    {
        this.Store.Features.TryGetValue(typeof(AuthState).FullName!, out var feature);

        return (AuthState)feature!.GetState();
    }
}