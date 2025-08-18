using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp_06.Models;

namespace tp_06.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult verificarUsuario(string UsuarioIngresado, string PasswordIngresada)
    {
        Usuario usuario = BD.VerificarCuenta(UsuarioIngresado, PasswordIngresada);
        if (usuario != null)
        {
            HttpContext.Session.SetString("usuario", Objeto.ObjectToString(usuario));
            return RedirectToAction("Logged", "Home");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return RedirectToAction("Index", "Home");
        }
    }
    [HttpGet]
    public IActionResult Registrarse()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registrarse(string NombreUsuarioNuevo, string PasswordNuevo)
    {
        int sePudo = BD.CrearUsuario(NombreUsuarioNuevo, PasswordNuevo);
        if (sePudo == 1)
        {
            Usuario usuario = BD.VerificarCuenta(NombreUsuarioNuevo, PasswordNuevo);
            HttpContext.Session.SetString("usuario", Objeto.ObjectToString(usuario));
            ViewBag.NombreUsuario = usuario.NombreUsuario;
            return RedirectToAction("Logged", "Home");
        }
        else
        {

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View("Index");

        }
    }
    public IActionResult Error()
    {
        ViewBag.Mensaje = "Usuario o contraseña erroneos";
        return View();
    }
    public IActionResult CompartirTarea(Tarea tarea, string UsuarioCompartir){
        Usuario usuario = BD.ObtenerUsuarioPorNombre(UsuarioCompartir);
        ViewBag.Compartido = BD.CompartirTarea(usuario, tarea);
    return RedirectToAction("Logged", "Home"); 
    }
}