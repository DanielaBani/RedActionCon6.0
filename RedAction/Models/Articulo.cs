using RedAction.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace RedAction.Models
{
    public class Articulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Autor")]
        public string AutorId { get; set; }
        [ForeignKey("AutorId")]
        [Display(Name = "Autor")]
        public Usuario? autor { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(50, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(1000, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        [Display(Name = "Escribí tu artículo")]
        public string contenido { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [Display(Name = "¿Sobre qué tema vas a hablar?")]
        [EnumDataType(typeof(Seccion))]
        public Seccion seccion { get; set; }
        [Display(Name = "Estado del Artículo")]
        [EnumDataType(typeof(EstadoArticulo))]
        public EstadoArticulo? estado { get; set; }
        [Display(Name = "Observaciones")]
        public string? observaciones { get; set; }
    }
}
