namespace CRUD.Example
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.Core;
    using CRUD.CQRS;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class GetExampleTextsByIdsQuery : IQuery<ExampleTextDto[]>
    {
        #region Properties

        public int[] Ids { get; init; }

        public bool ToUpper { get; set; }

        #endregion

        #region Nested Classes

        class Handler : QueryHandlerBase<GetExampleTextsByIdsQuery, ExampleTextDto[]>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task<ExampleTextDto[]> Execute(GetExampleTextsByIdsQuery request, CancellationToken cancellationToken)
            {
                var entities = await Repository<ExampleEntity>().Get(new EntitiesByIdsSpec<ExampleEntity, int>(request.Ids)).ToArrayAsync(cancellationToken);

                var dtos = this.Mapper.Map<ExampleTextDto[]>(entities);

                if (request.ToUpper)
                    Parallel.ForEach(dtos, dto => dto.Text = dto.Text.ToUpper());

                return dtos;
            }
        }

        #endregion
    }
}