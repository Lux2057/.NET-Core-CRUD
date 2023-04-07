namespace CRUD.Example
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public class CustomExampleController : DispatcherControllerBase
    {
        #region Constructors

        public CustomExampleController(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetTexts(int[] ids, bool toUpper, CancellationToken cancellationToken = default)
        {
            var dtos = await this.Dispatcher.QueryAsync(new GetExampleTextsByIdsQuery()
                                                        {
                                                                Ids = ids,
                                                                ToUpper = toUpper
                                                        }, cancellationToken);

            return Ok(dtos);
        }

        [HttpPut]
        public async Task<IActionResult> EditText([FromBody] ExampleTextDto item, CancellationToken cancellationToken = default)
        {
            var command = new EditExampleTextByIdCommand { Dto = item };
            await this.Dispatcher.PushAsync(command, cancellationToken);

            return Ok(command.Result);
        }
    }
}