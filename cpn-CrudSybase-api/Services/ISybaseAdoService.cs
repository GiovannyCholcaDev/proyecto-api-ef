using cpn_CrudSybase_api.Entities.Request;
using cpn_CrudSybase_api.Entities.Response;

namespace cpn_CrudSybase_api.Services
{
    public interface ISybaseAdoService
    {
        Task<TestConSybaseListResponse> ListaSybase(ClienteRequest request);
    }
}
