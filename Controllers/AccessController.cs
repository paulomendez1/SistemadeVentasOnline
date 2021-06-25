using SistemadeVentasOnline.DB;
using System;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;

namespace SistemadeVentasOnline.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult CheckLogin(string email, string password)
        {
            SistemadeVentasEntities db = new SistemadeVentasEntities();
            string pwDes = Common.Encrypt.GetSHA256(password);
            var dataItem = db.Usuarios.Where(x => x.Email == email && x.Password == pwDes).SingleOrDefault();
            string isLogged = null;
            if (dataItem != null)
            {
                Session["User"] = dataItem.Email;
                Session["Rol"] = dataItem.RolId;
                if (dataItem.RolId == 1)
                {
                    isLogged = "admin";
                }
                else
                {
                    isLogged = "user";
                }

            }
            else
            {
                isLogged = "";
            }

            return Json(isLogged, JsonRequestBehavior.AllowGet);
        }



        public ActionResult SignUp(string firstname, string lastname, string email, string password, string password2)
        {
            //try
            //{
            if (email != null)
            {
                using (SistemadeVentasEntities db = new SistemadeVentasEntities())
                {
                    var oUsuario = new DB.Usuario();
                    var securePw = Common.Encrypt.ContrasenaSegura(password);
                    var correo = (from d in db.Usuarios
                                  where d.Email == email
                                  select d).ToList();
                    if (ModelState.IsValid)
                    {
                        if (correo.Count > 0)
                        {
                            ViewBag.Duplicate = "El email" + oUsuario.Email + "ya existe.";
                        }
                        else if (password != password2)
                        {
                            Response.Write("<script>alert('Las contraseñas no coinciden')</script>");
                        }
                        else if (securePw == false)
                        {
                            Response.Write("<script>alert('La contraseña debe contener como minimo 8 caracteres, y al menos un caracter en mayuscula, uno en minuscula y un numero')</script>");
                        }
                        else
                        {
                            oUsuario.Activo = true;
                            oUsuario.Apellido = lastname;
                            oUsuario.CreadoEn = DateTime.Now;
                            oUsuario.Eliminado = false;
                            oUsuario.Email = email;
                            oUsuario.ModificadoEn = null;
                            oUsuario.Nombre = firstname;
                            oUsuario.Password = Common.Encrypt.GetSHA256(password);
                            oUsuario.RolId = 2;
                            db.Usuarios.Add(oUsuario);
                            db.SaveChanges();
                            Common.EmailOperations.EnviarEmailRegistro(email, password, firstname);
                            return Redirect("~/Access/Login");
                        }

                    }
                    return View();
                }
            }
            else
            {
                return View();
            }
            //}
            //catch
            //{
            //    Response.Write("<script>alert('Registrese')</script>");
            //    return View();
            //}
        }

        public ActionResult RecoveryPw(string email)
        {
            return View();
        }


        public ActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPassword(DB.Usuario model)
        {
            SistemadeVentasEntities db = new SistemadeVentasEntities();
            var emailactivo = Session["User"].ToString();
            var list = (from d in db.Usuarios
                        where d.Email == emailactivo
                        select d).ToList();
            var idactivo = list.Select(f => f.UsuarioId);
            if (ModelState.IsValid)
            {
                var oProducto = db.Usuarios.Find(idactivo);
                oProducto.Password = model.Password;
                db.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Redirect("~/User/UserIndex");
            }
            return Redirect("~/User/UserIndex");

            /*catch
            {
                Response.Write("<script>alert('La categoria ingresada ya existe!')</script>");
                return View(model);
            }*/
        }
    }
}