using SistemadeVentasOnline.DB;
using SistemadeVentasOnline.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Categoria = SistemadeVentasOnline.Models.Categoria;

namespace SistemadeVentasOnline.Controllers
{
    [AuthorizationFilter]
    public class AdminController : Controller
    {
        [AuthorizationFilter]
        public ActionResult Dashboard()
        {
            ViewBag.items = GetCategory();
            return View();
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index","Home");
        }
        public List<SelectListItem> GetCategory()
        {
            List<Categoria> lst;
            List<SelectListItem> lista = new List<SelectListItem>();
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                lst = (from d in db.Categorias
                       select new Categoria
                       {
                           CategoriaId = d.CategoriaId,
                           CategoriaNombre = d.CategoriaNombre
                       }).ToList();
            }
            foreach (var item in lst)
            {
                lista.Add(new SelectListItem { Value = item.CategoriaId.ToString(), Text = item.CategoriaNombre.ToString() });
            }
            return lista;
        }

        #region CATEGORIAS
        [AuthorizationFilter]
        public ActionResult Categorias()
        {
            List<Categoria> lst;
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                lst = (from d in db.Categorias
                       select new Categoria
                       {
                           CategoriaId = d.CategoriaId,
                           CategoriaNombre = d.CategoriaNombre
                       }).ToList();
            }

            return View(lst);
        }

        [HttpGet]
        public ActionResult AgregarCategoria()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarCategoria(Categoria model)
        {
            try
            {
                using (SistemadeVentasEntities db = new SistemadeVentasEntities()) {
                    var oCategoria = new DB.Categoria();
                    var nombres = (from d in db.Categorias
                                   where d.CategoriaNombre == oCategoria.CategoriaNombre
                                   select d).ToList();
                    if (ModelState.IsValid)
                    {
                        if (nombres.Count > 0)
                        {
                            ViewBag.Duplicate = "El nombre de categoria" + oCategoria.CategoriaNombre + "ya existe.";
                        }
                        else
                        {
                            oCategoria.CategoriaNombre = model.CategoriaNombre;
                            oCategoria.Activo = true;
                            oCategoria.Eliminado = false;

                            db.Categorias.Add(oCategoria);
                            db.SaveChanges();
                        }
                        return Redirect("~/Admin/Categorias");
                    }
                    return View(model);
                }
            }
            catch
            {
                Response.Write("<script>alert('La categoria ingresada ya existe!')</script>");
                return View(model);
            }
        }

        public ActionResult EditarCategoria(int Id)
        {
            Categoria model = new Categoria();
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                var oCategoria = db.Categorias.Find(Id);
                model.CategoriaNombre = oCategoria.CategoriaNombre;
                model.CategoriaId = oCategoria.CategoriaId;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditarCategoria(Categoria model)
        {
            try
            {
                SistemadeVentasEntities db = new SistemadeVentasEntities();
                var oCategoria = db.Categorias.Find(model.CategoriaId);
                if (ModelState.IsValid)
                {
                    oCategoria.CategoriaNombre = model.CategoriaNombre;
                    oCategoria.Activo = true;
                    oCategoria.Eliminado = false;

                    db.Entry(oCategoria).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return Redirect("~/Admin/Categorias");
                }
                return View(model);
            }
            catch
            {
                Response.Write("<script>alert('La categoria ingresada ya existe!')</script>");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult EliminarCategoria(int Id)
        {
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                var oCategoria = db.Categorias.Find(Id);
                db.Categorias.Remove(oCategoria);
                db.SaveChanges();
            }
            return Redirect("~/Admin/Categorias");
        }

        #endregion CATEGORIAS

        #region PRODUCTOS
        [AuthorizationFilter]
        public ActionResult Productos()
        {
            List<Models.Producto> lst;
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                lst = (from d in db.Productoes
                       select new Models.Producto
                       {
                           ProductoId = d.ProductoId,
                           ProductoNombre = d.ProductoNombre,
                           Cantidad = d.Cantidad
                       }).ToList();
            }

            return View(lst);
        }

        [HttpGet]
        public ActionResult AgregarProducto()
        {
            ViewBag.items = GetCategory();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarProducto(Models.Producto model, HttpPostedFileBase file)
        {
            string pic = null;
            string path = null;
            try
            {
                SistemadeVentasEntities db = new SistemadeVentasEntities();
                var oProducto = new DB.Producto();
                var nombres = (from d in db.Productoes
                               where d.ProductoNombre == oProducto.ProductoNombre
                               select d).ToList();
                if (ModelState.IsValid)
                {
                    if (nombres.Count > 0)
                    {
                        ViewBag.Duplicate = "El nombre del producto" + oProducto.ProductoNombre + "ya existe.";
                    }
                    else
                    {
                        if (file != null)
                        {
                            pic = System.IO.Path.GetFileName(file.FileName);
                            path = System.IO.Path.Combine(Server.MapPath("~/ProductImg"), pic);
                            file.SaveAs(path);
                        }

                        oProducto.ProductoNombre = model.ProductoNombre;
                        oProducto.CategoriaId = model.CategoriaId;
                        oProducto.Activo = true;
                        oProducto.Eliminado = false;
                        oProducto.FechaCreacion = DateTime.Now;
                        oProducto.FechaModificacion = null;
                        oProducto.Description = model.Description;
                        oProducto.ProductImage = pic;
                        oProducto.Cantidad = model.Cantidad;
                        oProducto.Precio = (decimal)model.Precio;

                        db.Productoes.Add(oProducto);
                        db.SaveChanges();
                    }
                    return Redirect("~/Admin/Productos");
                }
                return View(model);
            }
            catch
            {
                Response.Write("<script>alert('El producto ingresado ya existe!')</script>");
                return View(model);
            }
        }

        public ActionResult EditarProducto(int Id)
        {
            ViewBag.items = GetCategory();
            Models.Producto model = new Models.Producto();
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                var oProducto = db.Productoes.Find(Id);
                model.ProductoNombre = oProducto.ProductoNombre;
                model.CategoriaId = oProducto.CategoriaId;
                model.Activo = oProducto.Activo;
                model.Eliminado = oProducto.Eliminado;
                model.FechaCreacion = oProducto.FechaCreacion;
                model.FechaModificacion = oProducto.FechaModificacion;
                model.Description = oProducto.Description;
                model.ProductImage = oProducto.ProductImage;
                model.Cantidad = oProducto.Cantidad;
                model.Precio = oProducto.Precio;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditarProducto(DB.Producto model, int id, HttpPostedFileBase file)
        {
            string pic = null;
            string path = null;

            if (ModelState.IsValid)
            {
                SistemadeVentasEntities db = new SistemadeVentasEntities();

                var oProducto = db.Productoes.Find(id);

                if (file != null)
                {
                    pic = System.IO.Path.GetFileName(file.FileName);
                    path = System.IO.Path.Combine(Server.MapPath("~/ProductImg"), pic);
                    file.SaveAs(path);
                }
                oProducto.ProductoNombre = model.ProductoNombre;
                oProducto.CategoriaId = model.CategoriaId;
                oProducto.Activo = true;
                oProducto.Eliminado = false;
                //oProducto.FechaCreacion = DateTime.Now;
                oProducto.FechaModificacion = DateTime.Now;
                oProducto.Description = model.Description;
                oProducto.ProductImage = pic;
                oProducto.Cantidad = model.Cantidad;
                oProducto.Precio = (decimal)model.Precio;

                db.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Redirect("~/Admin/Productos");
            }
            return View(model);

            /*catch
            {
                Response.Write("<script>alert('La categoria ingresada ya existe!')</script>");
                return View(model);
            }*/
        }

        [HttpGet]
        public ActionResult EliminarProducto(int Id)
        {
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                var oProducto = db.Productoes.Find(Id);
                db.Productoes.Remove(oProducto);
                db.SaveChanges();
            }
            return Redirect("~/Admin/Productos");
        }

        #endregion PRODUCTOS
    }
}