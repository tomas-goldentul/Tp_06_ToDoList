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
        return View();
    }
      public IActionResult Logged()
    {
        if(HttpContext.Session.GetString("usuario") != null){
            Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario "));
            ViewBag.NombreUsuario = usuario.NombreUsuario;
        }
        return View();
    }
}
