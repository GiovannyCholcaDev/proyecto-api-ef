using cpn_CrudSybase_api.Entities.Dto;
using cpn_CrudSybase_api.Entities.Request;
using cpn_CrudSybase_api.Entities.Response;
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
            TestConSybaseListResponse response = await _service.SybaseLista(param);

            return Ok(response);

        }


        [HttpPost("InsertarDataSyBase")]
        public async Task<IActionResult> InsertarDataSyBase([FromBody] TestConSybaseRequest param)
        {
            bool response = await _service.SybaseInsert(param);
            return Ok(response);
        }


        [HttpPost("UpdateDataSyBase")]
        public async Task<IActionResult> UpdateDataSyBase([FromBody] TestConSybaseRequest param)
        {
            bool response = await _service.SybaseUpdate(param);
            return Ok(response);
        }

        [HttpPost("DeleteDataSyBase")]
        public async Task<IActionResult> DeleteDataSyBase([FromBody] ClienteRequest param)
        {
            bool response = await _service.SybaseDelete(param);
            return Ok(response);
        }


    }
}
