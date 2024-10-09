using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment
{
    public class Publicidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? imageUrl { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage ="Debe contener al menos 40 caracteres")]
        public string? Titulo { get; set; }


    }
}
