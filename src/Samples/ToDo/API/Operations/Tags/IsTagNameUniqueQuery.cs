namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class IsTagNameUniqueQuery : QueryBase<bool>
{
    #region Properties

    public string Name { get; }

    #endregion

    #region Constructors

    public IsTagNameUniqueQuery(string name)
    {
        Name = name;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<IsTagNameUniqueQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(IsTagNameUniqueQuery request, CancellationToken cancellationToken)
        {
            return !await Repository.Read(new NameProp.FindBy.EqualTo<TagEntity>(request.Name)).AnyAsync(cancellationToken);
        }
    }

    #endregion
}