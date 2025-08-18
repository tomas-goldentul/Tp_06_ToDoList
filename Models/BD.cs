using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
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
  public static Usuario ObtenerUsuarioPorNombre(string NombreUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @UNombre";
        
        // Assuming 'Usuario' is your model class
        var usuario = connection.QueryFirstOrDefault<Usuario>(query, new { UNombre = NombreUsuario });

        return usuario;
    }
}

    public static List<Tarea> ObtenerTareas(Usuario usuario)
    {
        if (usuario != null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Tareas INNER JOIN TareasXUsuarios on ID = IDTarea WHERE IDUsuario = @UID";
                Tareas = connection.Query<Tarea>(query, new { UID = usuario.ID }).ToList();
            }
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
                break;
            }
        }
        return UsuarioEncontrado;
    }
    public static int CrearUsuario(string nombreUsuarioNuevo, string passwordnuevo)
    {
        int registrosAfectados = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "CrearUsuario";
            registrosAfectados = connection.Execute
            (storedProcedure, new { NombreUsuario = nombreUsuarioNuevo, Password = passwordnuevo },
                commandType: CommandType.StoredProcedure
            );
        }
        return registrosAfectados;

    }
    public static bool CompartirTarea(Usuario usuario, Tarea tarea){
        TareaXUsuario registrosAfectados;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO TareasXUsuarios (IDUsuario, IDTarea, EsDueño) VALUES (@UID , @TID , 0)";
            registrosAfectados = connection.QueryFirstOrDefault(query, new { UID = usuario.ID, TID = tarea.ID });
        }
        if (registrosAfectados != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool EsDueño(Tarea tarea, Usuario usuario)
    {
        TareaXUsuario registrosAfectados;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM TareasXUsuarios where @UID = IDUsuario and @TID = IDTarea and EsDueño is true";
            registrosAfectados = connection.QueryFirstOrDefault(query, new { UID = usuario.ID, TID = tarea.ID });
        }
        if (registrosAfectados != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int EditarTarea(Tarea tarea, Usuario usuario, string descripcion)
    {
        int registrosAfectados = 0;
        if (EsDueño(tarea, usuario))
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE TAREAS SET Descripcion = @TDesc where ID = @TID";
                registrosAfectados = connection.Execute(query, new { TID = tarea.ID, TDesc = descripcion });
            }
        }
        return registrosAfectados;
    }
    public static int EliminarTarea(int ID)
    {
        int registrosAfectados = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE TAREAS SET Eliminada = 1 WHERE ID = @pID";
            registrosAfectados = connection.Execute(query, new { pID = ID });
        }
        return registrosAfectados;
    }
    public static int CrearTareas(string descripcion, int IDUsuario)
    {
        int registrosAfectados = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "CrearTarea";  
            registrosAfectados = connection.Execute
            (
                storedProcedure,
                new { pDescripcion = descripcion, pEliminada = false, pFinalizada = false, IdUsuario = IDUsuario }, 
                commandType: CommandType.StoredProcedure
            );
        }
        return registrosAfectados;
    }

public static int FinalizarTarea(int IDTarea, bool Finalizada)
{
    int registrosAfectados = 0;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "UPDATE TAREAS SET Finalizada = @pFinalizada WHERE ID = @pID";
        registrosAfectados = connection.Execute(query, new { pID = IDTarea, pFinalizada = Finalizada });
    }
    return registrosAfectados;
}

    public static Tarea ObtenerTareaPorID(int tareaID)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM Tareas WHERE ID = @ID";
        return connection.QueryFirstOrDefault<Tarea>(query, new { ID = tareaID });
    }
}

}