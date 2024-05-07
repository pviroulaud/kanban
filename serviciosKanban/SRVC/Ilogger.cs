using serviciosKanban.DTO;


namespace serviciosKanban.SRVC
{
    public interface Ilogger
    {
        void registrarAlta(int usuarioId,string entidad,int entidadId,string? detalle= null);
        void registrarBaja(int usuarioId,string entidad,int entidadId,string? detalle= null);
        void registrarModificacion(int usuarioId,string entidad,int entidadId,string? detalle= null);
    }
    
}