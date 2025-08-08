public class TareaXUsuario{
    public int IDTarea{get;set;}
    public int IDUsuario{get;set;}
    public bool EsDueño{get;set;}
    public TareaXUsuario (int IDTarea, int IDUsuario, bool EsDueño){
        this.IDTarea = IDTarea;
        this.IDUsuario = IDUsuario;
        this.EsDueño = EsDueño;
    }
    }