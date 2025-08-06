using Microsoft.Data.SqlClient;
using Dapper;
public class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=TP06_Goldentul_Gartenkrot; Integrated Security=True; TrustServerCertificate=True;";
    public static List<Usuario> Usuarios = new List<Usuario>();
    public static List<Tarea> Tareas = new List<Tarea>();
    public static List<Usuario> ObtenerUsuarios()

    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios";
            Usuarios = connection.Query<Usuario>(query).ToList();
        }
        return Usuarios;
    }
    public static List<Tarea> ObtenerTareas(Usuario usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Tareas inner join TareasXUsuarios on ID = IDTarea where IDUsuario = @PID";
            Tareas = connection.Query<Tarea>(query, new{PID = usuario.ID}).ToList();
        }
        return Tareas;
    }
    public static Usuario VerificarCuenta(string UsuarioIngresado, string PasswordIngresada)
    {
        Usuario UsuarioEncontrado = null;
        foreach (Usuario usu in Usuarios)
        {
            if (UsuarioIngresado == usu.NombreUsuario && PasswordIngresada == usu.Password)
            {
                UsuarioEncontrado = usu;
            }
        }
        return UsuarioEncontrado;
    }
    public static int CrearUsuario(string nombreUsuarioNuevo, string passwordnuevo)
    {
        int registrosAfectados = 0;
        string query = "INSERT INTO Usuarios (NombreUsuario, Password) VALUES (@pNombre, @pPassword)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            registrosAfectados = connection.Execute(query, new{pNombre = nombreUsuarioNuevo, pPassword = passwordnuevo});
        }
        return registrosAfectados;
    }
}