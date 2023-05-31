namespace Examples.WebAPI
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.WebAPI;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    [Route("[controller]/[action]")]
    public class CustomExampleController : DispatcherControllerBase
    {
        #region Constructors

        public CustomExampleController(IDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetTexts(int[] ids, bool toUpper, CancellationToken cancellationToken = default)
        {
            var dtos = await QueryAsync(new GetExampleTextsByIdsQueryBase
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
            await PushAsync(command, cancellationToken);

            return Ok(command.Result);
        }
    }
}