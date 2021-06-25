using SistemadeVentasOnline.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemadeVentasOnline.Models.User

{
    public class Item
    {
        public  DB.Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public SelectList Productos { get; set; }
    }
}