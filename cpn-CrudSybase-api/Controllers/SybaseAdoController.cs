using cpn_CrudSybase_api.Entities.Request;
using cpn_CrudSybase_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace cpn_CrudSybase_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SybaseAdoController : Controller
    {

        private readonly ISybaseAdoService _service;

        public SybaseAdoController(ISybaseAdoService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }


        [HttpPost("ConsultarDataSyBase")]
        public async Task<IActionResult> ConsultarDataSyBase([FromBody] ClienteRequest param)
        {
            var response = _service.ListaSybase(param);

            return Ok(response);

        }
    }
}
