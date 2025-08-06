public class Tarea{
    public int ID{get;set;}
    public string Descripcion{get;set;}
    public bool Eliminada{get;set;}
    public bool Finalizada{get;set;}
    public Tarea (int ID, string Descripcion, bool Eliminada, bool Finalizada){
        this.ID = ID;
        this.Descripcion = Descripcion;
        this.Eliminada = Eliminada;
        this.Finalizada = Finalizada;
    }
}