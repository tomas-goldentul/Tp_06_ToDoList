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
    [HttpPost]
    public IActionResult CompartirTarea(Tarea tarea, string UsuarioCompartir)
    {
        Usuario usuarioActual = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        
        if (usuarioActual == null)
        {
            TempData["Error"] = "Usuario no autenticado.";
            return RedirectToAction("Index", "Home");
        }
        
        if (string.IsNullOrWhiteSpace(UsuarioCompartir))
        {
            TempData["Error"] = "Debe especificar un usuario para compartir.";
            return RedirectToAction("Logged", "Home");
        }
        
        if (usuarioActual.NombreUsuario.ToLower() == UsuarioCompartir.ToLower())
        {
            TempData["Error"] = "No puede compartir una tarea consigo mismo.";
            return RedirectToAction("Logged", "Home");
        }
        
        if (!BD.EsDueño(tarea, usuarioActual))
        {
            TempData["Error"] = "Solo puede compartir tareas de las que sea dueño.";
            return RedirectToAction("Logged", "Home");
        }
        
        Usuario usuarioCompartir = BD.ObtenerUsuarioPorNombre(UsuarioCompartir);
        if (usuarioCompartir == null)
        {
            TempData["Error"] = "Usuario no encontrado.";
            return RedirectToAction("Logged", "Home");
        }
        
        if (BD.TareaCompartidaConUsuario(tarea, usuarioCompartir))
        {
            TempData["Error"] = "La tarea ya está compartida con este usuario.";
            return RedirectToAction("Logged", "Home");
        }
        
        bool compartido = BD.CompartirTarea(usuarioCompartir, tarea);
        if (compartido)
        {
            TempData["Mensaje"] = $"Tarea compartida exitosamente con {UsuarioCompartir}.";
        }
        else
        {
            TempData["Error"] = "No se pudo compartir la tarea.";
        }
        
        return RedirectToAction("Logged", "Home");
    }
}