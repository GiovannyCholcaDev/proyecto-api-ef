using System.ComponentModel.DataAnnotations;

namespace cpn_CrudSybase_api.Entities.Request
{
    public class ClienteRequest
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio para realizar la consulta")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El valor ingresado en el campo {0} no es correcto")]
        public string? Identificacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio para realizar la consulta")]
        public int Id { get; set; }
    }

    public class TestConSybaseRequest
    {
        public int Id { get; set; }

        public string? Identificacion { get; set; }

        public string? Nombre { get; set; }

        public DateTime Fecha { get; set; }

        public string? Estado { get; set; }

    }
}
