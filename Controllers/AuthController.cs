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
    public IActionResult verificarUsuario(string UsuarioIngresado, int PasswordIngresada){
        Usuario usuario = BD.VerificarCuenta(UsuarioIngresado, PasswordIngresada);
        if (usuario != null){
            
        }
    }
}