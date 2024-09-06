using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.PaypalOrder.Dto
{
    public class MatriculaDto
    {
        public int? Id { get; set; }
        [Required]
        public int CursoId { get; set; }
        [Required]
        public string EstudianteId { get; set; }
        public bool? IsActive { get; set; }
        public string? Estado { get; set; }
        public double? NotaFinal { get; set; }
    }
}
