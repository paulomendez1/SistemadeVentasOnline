using SistemadeVentasOnline.DB;
using SistemadeVentasOnline.Models.Home;
using System.Web.Mvc;

namespace SistemadeVentasOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string search, int? page)
        {
            HomeIndexView model = new HomeIndexView();
            return View(model.CreateModel(search, 4, page));
        }

        public ActionResult Categorias(string search, int? page)
        {
            HomeIndexView model = new HomeIndexView();
            SistemadeVentasEntities db = new SistemadeVentasEntities();
            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaId", "CategoriaNombre");
            return View(model.CreateModel(search, 4, page));


        }
    }
}