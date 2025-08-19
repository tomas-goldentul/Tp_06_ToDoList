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
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        
        if (usuario == null)
        {
            TempData["Error"] = "Debe iniciar sesión para acceder a esta página.";
            return RedirectToAction("Index", "Home");
        }

        List<Tarea> listaTareas = BD.ObtenerTareas(usuario);
        ViewBag.NombreUsuario = usuario.NombreUsuario;
        ViewBag.ListaTareas = listaTareas;
        ViewBag.UsuarioActual = usuario;
        
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
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        
        if (usuario == null)
        {
            TempData["Error"] = "Usuario no autenticado.";
            return RedirectToAction("Index", "Home");
        }
        
        Tarea tarea = BD.ObtenerTareaPorID(ID);
        
        if (tarea == null)
        {
            TempData["Error"] = "Tarea no encontrada.";
            return RedirectToAction("Logged");
        }
        
        if (!BD.EsDueño(tarea, usuario))
        {
            TempData["Error"] = "Solo puede eliminar tareas de las que sea dueño.";
            return RedirectToAction("Logged");
        }
        
        int resultado = BD.EliminarTarea(ID);
        if (resultado > 0)
        {
            TempData["Mensaje"] = "Tarea eliminada exitosamente.";
        }
        else
        {
            TempData["Error"] = "No se pudo eliminar la tarea.";
        }
        
        return RedirectToAction("Logged");
    }
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
public IActionResult FinalizarTarea(int ID)
{
    BD.FinalizarTarea(ID, true); 
    return RedirectToAction("Logged");
}

[HttpPost]
public IActionResult ModificarDescripcion(int ID, string NuevaDescripcion)
{
    Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
    
    if (usuario == null)
    {
        TempData["Error"] = "Usuario no autenticado.";
        return RedirectToAction("Index", "Home");
    }
    
    Tarea tarea = BD.ObtenerTareaPorID(ID);
    
    if (tarea == null)
    {
        TempData["Error"] = "Tarea no encontrada.";
        return RedirectToAction("Logged");
    }
    
    if (string.IsNullOrWhiteSpace(NuevaDescripcion))
    {
        TempData["Error"] = "La descripción no puede estar vacía.";
        return RedirectToAction("Logged");
    }
    
    int resultado = BD.EditarTarea(tarea, usuario, NuevaDescripcion);
    if (resultado > 0)
    {
        TempData["Mensaje"] = "Tarea modificada exitosamente.";
    }
    else
    {
        TempData["Error"] = "No se pudo modificar la tarea. Verifique que sea el dueño de la tarea.";
    }
    
    return RedirectToAction("Logged");
}

public IActionResult Logout()
{
    HttpContext.Session.Clear();
    TempData["Mensaje"] = "Sesión cerrada exitosamente.";
    return RedirectToAction("Index", "Home");
}
}
