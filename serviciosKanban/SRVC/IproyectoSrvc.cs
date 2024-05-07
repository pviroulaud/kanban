using serviciosKanban.DTO;


namespace serviciosKanban.SRVC
{
    public interface IproyectoSrvc
    {
        List<proyectoDTO> listar();

        int nuevo(int usuarioOperacionId,nuevoProyectoDTO datos);
        int actualizar(int usuarioOperacionId,proyectoDTO datos);

        bool eliminar(int usuarioOperacionId,int id);

        proyectoDTO? obtener(int id);
    }
    
}