namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

[UsedImplicitly]
public class ValidationMiddleware : Middleware
{
    public override async Task InitializeAsync(IDispatcher dispatcher, IStore store)
    {
        await base.InitializeAsync(dispatcher, store);

        store.UnhandledException += (_, exceptionArgs) =>
                                    {
                                        if (exceptionArgs.Exception.GetType() == typeof(ValidationException))
                                        {
                                            var exception = exceptionArgs.Exception as ValidationException;

                                            dispatcher.Dispatch(new SetValidationStateWf.Init(exception!.ValidationFailure));
                                        }
                                        else
                                        {
                                            throw exceptionArgs.Exception;
                                        }
                                    };
    }
}