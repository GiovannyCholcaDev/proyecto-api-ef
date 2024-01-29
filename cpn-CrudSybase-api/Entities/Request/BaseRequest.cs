using System.ComponentModel.DataAnnotations;

namespace cpn_CrudSybase_api.Entities.Request
{
    public class ClienteRequest
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio para realizar la consulta")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El valor ingresado en el campo {0} no es correcto")]
        public string? Identificacion { get; set; }
    }
}
