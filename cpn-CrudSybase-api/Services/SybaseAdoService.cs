using cpn_CrudSybase_api.Entities.Dto;
using cpn_CrudSybase_api.Entities.Request;
using cpn_CrudSybase_api.Entities.Response;
using cpn_CrudSybase_api.Util;
using Microsoft.Extensions.Options;
using System.Data;

namespace cpn_CrudSybase_api.Services
{
    public class SybaseAdoService : ISybaseAdoService
    {

        private readonly ConexionSyb _sqlSyBase = ConexionSyb.Instance;
        private readonly string _SybaseConnectionString;

        public SybaseAdoService(IOptions<Settings> options)
        {
            _SybaseConnectionString = options.Value.SybaseConnectionString!;

        }

        public async Task<bool> SybaseDelete(ClienteRequest request)
        {
            string cadenaSql = "delete from testConSybase where id = @id";
            Dictionary<string, object> bag = new()
            {
               { "@id", request.Id}
            };


            return await _sqlSyBase.exec(cadenaSql, bag, _SybaseConnectionString);
        }

        public async Task<bool> SybaseInsert(TestConSybaseRequest testconSybase)
        {
            string cadenaSql = "INSERT INTO testConSybase (nombre, identificacion,fecha, estado) VALUES (@nombre, @identificacion, getdate(), '1')";
            Dictionary<string, object> bag = new()
            {
               { "@nombre", testconSybase.Nombre! },
               { "@identificacion", testconSybase.Identificacion! }
            };

            //var insertobj = _sqlSyBase.getInsert(cadenaSql, bag, _SybaseConnectionString);
            //await _sqlSyBase.exec(cadenaSql, CommandType.Text ,bag, _SybaseConnectionString);

            return await _sqlSyBase.exec(cadenaSql, bag, _SybaseConnectionString);
        }

        public async Task<TestConSybaseListResponse> SybaseLista(ClienteRequest request)
        {
            TestConSybaseListResponse result = new();
            string cadenaSql = "SELECT * FROM cob_ahorros..testConSybase where estado = @estado";
            
            Dictionary<string, object> bag = new()
            {
               { "@estado", "1" }
            };

            DataSet ds_response = await _sqlSyBase.GetDs(cadenaSql, bag, _SybaseConnectionString);
            DataTable dt_response = ds_response.Tables[0];
            if (dt_response.Rows.Count > 0)
            {
                List<TestConSybaseDTO> tablaSyBaseList = new List<TestConSybaseDTO>();
                tablaSyBaseList = (from DataRow dr in dt_response.Rows
                               select new TestConSybaseDTO()
                               {
                                   Id = Convert.ToInt32(dr["id"]),
                                   Identificacion = dr["nombre"].ToString(),
                                   Nombre = dr["identificacion"].ToString(),
                                   Fecha = DateTime.Parse(dr["fecha"].ToString()!),
                                   Estado = dr["estado"].ToString(),
                               }).ToList();

                result.Collection = tablaSyBaseList;
            }

            //TestConSybaseDTO testconSybase = new();

            return result;
        }

        public async Task<bool> SybaseUpdate(TestConSybaseRequest testconSybase)
        {
            string cadenaSql = "UPDATE testConSybase" +
                " SET nombre = @nombre, identificacion = @identificacion" +
                " WHERE id = @id";

            Dictionary<string, object> bag = new()
            {
               { "@nombre", testconSybase.Nombre! },
               { "@identificacion", testconSybase.Identificacion!},
               { "@id", testconSybase.Id }
            };

            return await _sqlSyBase.exec(cadenaSql, bag, _SybaseConnectionString);
        }


        public async Task<List<CuentaSpDto>> SybaseStoreProcedure(ClienteRequest request)
        {
            string cadenaSql = "efsp_consulta_test";
            Dictionary<string, object> bag = new()
            {
               { "@cedula", request.Identificacion! }
            };

            DataSet ds_response = await _sqlSyBase.ExecSP(cadenaSql, bag, _SybaseConnectionString);
            DataTable dt_response = ds_response.Tables[0];
            TestConSybaseListResponse result = new();
            List<CuentaSpDto> spList = new();
            if (dt_response.Rows.Count > 0)
            {
               
                spList = (from DataRow dr in dt_response.Rows
                          select new CuentaSpDto()
                          {
                              NombreCompleto = dr["cedula"].ToString(),
                              Cedula = dr["nombreCompleto"].ToString(),
                              Fecha = DateTime.Parse(dr["fecha"].ToString()!),
                              Email = dr["email"].ToString(),
                              Cuenta = dr["cuenta"].ToString(),
                              Valor = double.Parse(dr["cuenta"].ToString()!)
                          }).ToList();

            }
            return spList;
        }

    }
}
