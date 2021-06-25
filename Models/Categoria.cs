using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemadeVentasOnline.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la categoria es requerido")]
        [StringLength(100, ErrorMessage = "Minimo 3, Maximo 100 caracteres estan permitidos", MinimumLength = 3)]
        public string CategoriaNombre { get; set; }

        public Nullable<bool> Activo { get; set; }
        public Nullable<bool> Eliminado { get; set; }

        [NotMapped]
        public List<Categoria> CategoriaCollection { get; set; }
    }
}