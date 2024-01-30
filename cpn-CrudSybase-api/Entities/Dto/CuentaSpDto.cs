namespace cpn_CrudSybase_api.Entities.Dto
{
    public class CuentaSpDto
    {
        public string? Cedula { get; set; }

        public string? NombreCompleto { get; set; }

        public DateTime Fecha { get; set; }

        public string? Email { get; set; }

        public string? Cuenta { get; set; }

        public double Valor { get; set; }
    }
}
