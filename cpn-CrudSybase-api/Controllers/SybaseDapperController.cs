using cpn_CrudSybase_api.Entities.Dto;
using cpn_CrudSybase_api.Entities.Request;
using cpn_CrudSybase_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace cpn_CrudSybase_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SybaseDapperController : Controller
    {
        private readonly ISybaseAdoService _service;

        public SybaseDapperController(ISybaseAdoService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }


        [HttpPost("StoreProcedureDataSyBaseDapper")]
        public async Task<IActionResult> StoreProcedureDataSyBaseDapper([FromBody] ClienteRequest param)
        {
            List<CuentaSpDto> response = await _service.SybaseStoreProcedureDapper(param);
            return Ok(response);
        }
    }
}
