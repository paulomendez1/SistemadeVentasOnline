using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SistemadeVentasOnline.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100, ErrorMessage = "Minimo 3, Maximo 100 caracteres estan permitidos", MinimumLength = 3)]
        [Display(Name = "Nombre de producto")]
        public string ProductoNombre { get; set; }

        [Required]
        [Range(1, 50)]
        [Display(Name = "Categoria")]
        public Nullable<int> CategoriaId { get; set; }

        public Nullable<bool> Activo { get; set; }
        public Nullable<bool> Eliminado { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public Nullable<bool> IsFeatured { get; set; }

        [Required]
        [Range(typeof(int), "1", "500", ErrorMessage = "Cantidad Invalida")]
        public Nullable<int> Cantidad { get; set; }

        [Required]
        [Range(typeof(decimal), "1", "1000000", ErrorMessage = "Precio Invalido")]
        public Nullable<decimal> Precio { get; set; }

        public SelectList Categorias { get; set; }
    }
}