using System;
namespace RedAction.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        //Mensajes de Error
        public const string CampoRequerido = "{0} es obligatorio.";
        public const string CaracteresMinimos = "{0} debe tener al menos {1} caracteres.";
        public const string CaracteresMaximos = "{0} no debe superar los {1} caracteres.";
        public const string EmailInvalido = "{0} tiene formato inválido.";
    }
}