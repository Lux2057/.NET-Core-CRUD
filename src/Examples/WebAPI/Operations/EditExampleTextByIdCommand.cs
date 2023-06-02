namespace Examples.WebAPI
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.DAL.Abstractions;
    using FluentValidation;

    #endregion

    public class EditExampleTextByIdCommand : CommandBase
    {
        #region Properties

        public new bool Result { get; set; }

        public ExampleTextDto Dto { get; init; }

        #endregion

        #region Nested Classes

        public class Validator : AbstractValidator<EditExampleTextByIdCommand>
        {
            #region Constructors

            public Validator()
            {
                RuleFor(r => r.Dto).NotEmpty();
                RuleFor(r => r.Dto.Id).NotEmpty();
                RuleFor(r => r.Dto.Text).NotEmpty();
            }

            #endregion
        }

        class Handler : CommandHandlerBase<EditExampleTextByIdCommand>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(EditExampleTextByIdCommand command, CancellationToken cancellationToken)
            {
                var id = command.Dto.Id.GetValueOrDefault(0);

                var entity = Repository.Get(new FindEntityById<ExampleEntity, int>(id)).SingleOrDefault();

                if (entity == null)
                {
                    command.Result = false;
                    return;
                }

                entity.Text = command.Dto.Text.Trim();
                await Repository.UpdateAsync(entity, cancellationToken);

                command.Result = true;
            }
        }

        #endregion
    }
}