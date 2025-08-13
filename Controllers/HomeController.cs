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
        
        List<Tarea>listaTareas = new List<Tarea>();
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        if (usuario != null){
        listaTareas = BD.ObtenerTareas(usuario);
        ViewBag.NombreUsuario = usuario.NombreUsuario;
        ViewBag.ListaTareas = listaTareas;
        }
        return View();
    }
   public IActionResult CrearTarea(string Descripcion){
    Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
    int IDUsuario = usuario.IDUsuario;
    BD.CrearTareas(Descripcion, IDUsuario);
   }
}
