using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using RedAction.Models;

namespace RedAction.Models
{
    public class Usuario
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(7, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(8, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Dni { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(50, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        [Display(Name = "Nombre y Apellido")]
        public string nombreCompleto { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [Display(Name = "Email")]
        public string mail { get; set; }
        [Display(Name = "Tipo De Usuario")]
        [EnumDataType(typeof(TipoUsuario))]
        public TipoUsuario? tipo { get; set; }
        
        public List<Articulo>? ListaArticulos { get; set; }

        [Display(Name = "Foto de Perfil")]
        public string? FotoPerfil { get; set; }

    }
}
