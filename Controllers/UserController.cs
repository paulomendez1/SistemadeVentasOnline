using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemadeVentasOnline.DB;
using SistemadeVentasOnline.Filters;
using SistemadeVentasOnline.Models.User;
using SistemadeVentasOnline.Models.Home;

namespace SistemadeVentasOnline.Controllers
{
    [AuthorizationFilter]
    public class UserController : Controller
    {
        // GET: User
        [AuthorizationFilter]
        public ActionResult UserIndex(string search, int? page)
        {
            UserIndexView model = new UserIndexView();
            return View(model.CreateModel(search, 4, page));
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult UserCategorias(string search, int? page)
        {
            UserIndexView model = new UserIndexView();
            SistemadeVentasEntities db = new SistemadeVentasEntities();
            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaId", "CategoriaNombre");
            return View(model.CreateModel(search, 4, page));


        }

        [AuthorizationFilter]
        public ActionResult AddToCart(int productId, int? cantidad)
        {
            using (SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                if (cantidad <= 0 || cantidad == null)
                {
                    Response.Write("<script>alert('La categoria ingresada ya existe!')</script>");
                    return Redirect("UserIndex");
                }
                else
                {
                    if (Session["cart"] == null)
                    {
                        List<Item> cart = new List<Item>();
                        var product = db.Productoes.Find(productId);
                        cart.Add(new Item()
                        {
                            Producto = product,
                            Cantidad = (int)cantidad
                        });
                        if (cantidad > product.Cantidad)
                        {
                            Response.Write("<script>alert('La categoria ingresada ya existe!')</script>");
                        }
                        else
                        {
                            Session["cart"] = cart;
                        }

                    }
                    else
                    {
                        List<Item> cart = (List<Item>)Session["cart"];
                        var count = cart.Count();
                        var product = db.Productoes.Find(productId);
                        for (int i = 0; i < count; i++)
                        {
                            if (cart[i].Producto.ProductoId == productId)
                            {
                                int prevQty = cart[i].Cantidad;
                                cart.Remove(cart[i]);
                                cart.Add(new Item()
                                {
                                    Producto = product,
                                    Cantidad = (int)(prevQty + cantidad)
                                });

                            }
                            else
                            {
                                var prd = cart.Where(x => x.Producto.ProductoId == productId).SingleOrDefault();
                                if (prd == null)
                                {
                                    cart.Add(new Item()
                                    {
                                        Producto = product,
                                        Cantidad = (int)cantidad
                                    });
                                }
                            }
                        }
                    }
                    return Redirect("UserIndex");
                }
            }
            
        }

        [AuthorizationFilter]
        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Producto.ProductoId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            if (cart.Count > 0)
            {
                Session["cart"] = cart;
            }
            else
            {
                Session["cart"] = null;
            }
            return Redirect("UserIndex");
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult CheckoutDetails()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult DecreaseQty(int productId)
        {
            using(SistemadeVentasEntities db = new SistemadeVentasEntities())
            {
                if (Session["cart"] != null)
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    var product = db.Productoes.Find(productId);
                    foreach (var item in cart)
                    {
                        if (item.Producto.ProductoId == productId)
                        {
                            int prevQty = item.Cantidad;
                            if (prevQty > 0)
                            {
                                cart.Remove(item);
                                cart.Add(new Item()
                                {
                                    Producto = product,
                                    Cantidad = prevQty - 1
                                });
                            }
                            break;
                        }
                    }
                    Session["cart"] = cart;
                }
                return Redirect("Checkout");
            }
            
        }
    }
}