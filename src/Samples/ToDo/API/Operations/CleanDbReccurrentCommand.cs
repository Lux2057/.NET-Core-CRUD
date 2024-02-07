namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using Hangfire;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class CleanDbRecurrentCommand : CommandBase
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CleanDbRecurrentCommand>
    {
        #region Constants

        private const int deletingCount = 10;

        #endregion

        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CleanDbRecurrentCommand command, CancellationToken cancellationToken)
        {
            var tagsToDelete = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<TagEntity>(true)).Take(deletingCount).ToArrayAsync(cancellationToken);
            await Repository.DeleteAsync(tagsToDelete, cancellationToken);

            var statusesToDelete = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<StatusEntity>(true)).Take(deletingCount).ToArrayAsync(cancellationToken);
            await Repository.DeleteAsync(statusesToDelete, cancellationToken);

            var usersToDelete = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<UserEntity>(true)).Take(deletingCount).ToArrayAsync(cancellationToken);
            await Repository.DeleteAsync(usersToDelete, cancellationToken);

            var projectsToDelete = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<ProjectEntity>(true)).Take(deletingCount).ToArrayAsync(cancellationToken);
            await Repository.DeleteAsync(projectsToDelete, cancellationToken);

            var tasksToDelete = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<TaskEntity>(true)).Take(deletingCount).ToArrayAsync(cancellationToken);
            await Repository.DeleteAsync(tasksToDelete, cancellationToken);

            BackgroundJob.Schedule<IDispatcher>(dispatcher => dispatcher.PushAsync(new CleanDbRecurrentCommand(), new CancellationToken()), TimeSpan.FromMinutes(10));
        }
    }

    #endregion
}