using cpn_CrudSybase_api.Entities.Dto;
using cpn_CrudSybase_api.Entities.Request;
using cpn_CrudSybase_api.Entities.Response;

namespace cpn_CrudSybase_api.Services
{
    public interface ISybaseAdoService
    {
        Task<TestConSybaseListResponse> SybaseLista(ClienteRequest request);

        Task<bool> SybaseUpdate(TestConSybaseRequest testconSybase);

        Task<bool> SybaseDelete(ClienteRequest request);

        Task<bool> SybaseInsert(TestConSybaseRequest testconSybase);

        Task<List<CuentaSpDto>> SybaseStoreProcedure(ClienteRequest request);
    }
}
