namespace CRUD.Example
{
    #region << Using >>

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.Core;
    using CRUD.CQRS;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class EditExampleTextByIdCommand : CommandBase
    {
        #region Properties

        public new bool Result { get; set; }

        public ExampleTextDto Dto { get; init; }

        #endregion

        #region Nested Classes

        class Handler : CommandHandlerBase<EditExampleTextByIdCommand>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(EditExampleTextByIdCommand command, CancellationToken cancellationToken)
            {
                var id = command.Dto.Id.GetValueOrDefault(0);

                var entity = await Repository<ExampleEntity>().Get(new EntityByIdSpec<ExampleEntity>(id)).SingleOrDefaultAsync(cancellationToken);

                var t = this.Mapper.Map<ExampleDto>(entity);

                if (entity == null)
                {
                    command.Result = false;
                    return;
                }

                entity.Text = command.Dto.Text.Trim();
                await Repository<ExampleEntity>().AddOrUpdateAsync(entity, cancellationToken);

                command.Result = true;
            }
        }

        #endregion
    }
}