using serviciosKanban.DTO;


namespace serviciosKanban.SRVC
{
    public interface IincidenciaSrvc
    {
        List<idNombreDTO> listarNombreIncidencias();
        List<idNombreDTO> listarNombreIncidencias(int proyectoId);
        List<idNombreDTO> listarTipos();
        List<incidenciaCompletaDTO> listar();
        List<incidenciaCompletaDTO> listar(filtroBusquedaDTO filtros);
        int nuevo(int usuarioOperacionId,nuevaIncidenciaDTO datos);
        int actualizar(int usuarioOperacionId,incidenciaDTO datos);

        bool eliminar(int usuarioOperacionId,int id);

        incidenciaCompletaDTO? obtener(int id);

        List<idNombreDTO> listarEstados();
    }
    
}