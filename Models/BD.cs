using Microsoft.Data.SqlClient;
using Dapper;
System.Data;
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
            string query = "SELECT * FROM Tareas INNER JOIN TareasXUsuarios on ID = IDTarea WHERE IDUsuario = @UID";
            Tareas = connection.Query<Tarea>(query, new { UID = usuario.ID }).ToList();
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
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "CrearUsuario";
            registrosAfectados = connection.Execute
            (storedProcedure,new { NombreUsuario = nombreUsuarioNuevo, Password = passwordNuevo },
                commandType: CommandType.StoredProcedure
            );
        }
        return registrosAfectados;

    }
    public static bool EsDueño(Tarea tarea, Usuario usuario)
    {
        TareaXUsuario registrosAfectados;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM TareasXUsuarios where @UID = IDUsuario and @TID = IDTarea and EsDueño is true";
            registrosAfectados = connection.Query(query, new { UID = usuario.ID, TID = tarea.ID });
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
    public static int EliminarTarea(Tarea tarea)
    {
        int registrosAfectados = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE TAREAS SET Eliminada = 1 where ID = @pID";
            registrosAfectados = connection.Execute(query, new { pID = tarea.ID });
        }
        return registrosAfectados;
    }
    public static int CrearTareas(string descripcion)
    {
        int registrosAfectados = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Tareas (Descripcion, Eliminada, Finalizada) VALUES (@pDescripcion, @pEliminada, @pFinalizada)";
            registrosAfectados = connection.Execute(query, new { pDescripcion = descripcion, pEliminada = 0, pFinalizada = 0 });
        }
        return registrosAfectados;
    }
    public static int FinalizarTarea(int IDTarea, bool Finalizada)
    {
        int registrosAfectados = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE TAREAS SET Finalizada = @pFinalizada where ID = @pID";
            registrosAfectados = connection.Execute(query, new { pID = IDTarea , pFinalizada = Finalizada});
        }   
        return registrosAfectados;
    }
}