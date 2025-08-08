public class Usuario{
    public int ID {get;set;}
    public string NombreUsuario {get;set;}
    public string Password {get;set;}
    public Usuario (int ID, string NombreUsuario, string Password){
        this.ID = ID;
        this.NombreUsuario = NombreUsuario;
        this.Password = Password;
    }
}