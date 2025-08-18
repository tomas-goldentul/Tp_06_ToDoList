using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp_06.Models;

namespace tp_06.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        BD.ObtenerUsuarios();
        return View();
    }
    public IActionResult Logged()
    {

        List<Tarea> listaTareas = new List<Tarea>();
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        if (usuario != null)
        {
            listaTareas = BD.ObtenerTareas(usuario);
            ViewBag.NombreUsuario = usuario.NombreUsuario;
            ViewBag.ListaTareas = listaTareas;
        }
        return View();
    }
    public IActionResult CrearTarea(string Descripcion)
    {
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        int ID = usuario.ID;
        BD.CrearTareas(Descripcion, ID);
        ViewBag.listaTareas = BD.ObtenerTareas(usuario);
        return RedirectToAction("Logged");
    }
    public IActionResult EliminarTarea(int ID)
    {
        BD.EliminarTarea(ID);
        return RedirectToAction("Logged");
    }
    [HttpPost]
  [HttpPost]
    public IActionResult ProcesarTarea(int ID, bool tareaFinalizada, bool tareaEliminada)
    {
        Tarea tarea = BD.ObtenerTareaPorID(ID);
        if (tarea == null)
        {
            TempData["Mensaje"] = "La tarea no existe.";
            return RedirectToAction("Logged");
        }

        if (tareaFinalizada)
        {
            BD.FinalizarTarea(ID, true); 
        }

        if (tareaEliminada)
        {
            BD.EliminarTarea(ID); 
        }

        TempData["Mensaje"] = "Tarea procesada correctamente.";
        return RedirectToAction("Logged");
    }
 
[HttpPost]
public IActionResult FinalizarTarea(int ID, bool tareaFinalizada)
{
    BD.FinalizarTarea(ID, tareaFinalizada);
    
    return RedirectToAction("Logged");
}




}
