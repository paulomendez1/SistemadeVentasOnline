using System;
using System.ComponentModel.DataAnnotations;

namespace SistemadeVentasOnline.Models
{
    public class Compra
    {
        public int DetalleCompraId { get; set; }

        [Required]
        public Nullable<int> UsuarioId { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Ciudad { get; set; }

        [Required]
        public string Provincia { get; set; }

        [Required]
        public string Pais { get; set; }

        [Required]
        public string CodigoPostal { get; set; }

        public Nullable<int> OrdenId { get; set; }
        public Nullable<decimal> Monto { get; set; }

        [Required]
        public string TipoPago { get; set; }
    }
}