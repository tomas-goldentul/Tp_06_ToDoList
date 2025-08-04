using Microsoft.Data.SqlClient;
using Dapper;
public class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=TP06_Goldentul_Gartenkrot; Integrated Security=True; TrustServerCertificate=True;";
    public static List<Usuario> Usuarios = new List<Usuario>();
    public static List<Usuario> ObtenerUsuarios()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios";
            Usuarios = connection.Query<Usuario>(query).ToList();
        }
        return Usuarios;
    }
    public static Usuario VerificarCuenta(string UsuarioIngresado, int PasswordIngresada)
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
}