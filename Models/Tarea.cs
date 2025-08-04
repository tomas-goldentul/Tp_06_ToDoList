public class Tarea{
    public int Clave{get;set;}
    public string Descripcion{get;set;}
    public bool Eliminado{get;set;}
    public Usuario UsuarioDueño{get;set;}
    public List<Usuario> UsuarioCompartido{get;set;}
    public Tarea (int clave, string Descripcion, bool Eliminado, Usuario UsuarioDueño, List<Usuario> UsuarioCompartido){
        this.Clave = clave;
        this.Descripcion = Descripcion;
        this.Eliminado = Eliminado;
        this.UsuarioDueño = UsuarioDueño;
        this.UsuarioCompartido = UsuarioCompartido;
    }
    public bool VerificarTarea (Usuario NombreUsuario){
        if(NombreUsuario == UsuarioDueño || UsuarioCompartido.Contains(NombreUsuario)){
            return true;
        }
        else{
            return false;
        }
    }
}