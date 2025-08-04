public class Usuario{
    public string NombreUsuario {get;set;}
    public int Password {get;set;}
    public Usuario (string NombreUsuario, int Password){
        this.NombreUsuario = NombreUsuario;
        this.Password = Password;
    }
}