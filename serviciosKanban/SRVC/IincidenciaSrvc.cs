using serviciosKanban.DTO;


namespace serviciosKanban.SRVC
{
    public interface IincidenciaSrvc
    {
        List<idNombreDTO> listarNombreIncidenciasCerradas(int proyectoId);
        List<idNombreDTO> listarNombreIncidenciasCerradas();
        List<idNombreDTO> listarNombreIncidencias(bool incluirCerradas = false);
        List<idNombreDTO> listarNombreIncidencias(int proyectoId,bool incluirCerradas = false);
        List<idNombreDTO> listarTipos();
        List<incidenciaCompletaDTO> listar();
        List<incidenciaCompletaDTO> listar(filtroBusquedaDTO filtros, bool incluirCerradas = false);
        int nuevo(int usuarioOperacionId,nuevaIncidenciaDTO datos);
        int actualizar(int usuarioOperacionId,incidenciaDTO datos);

        bool eliminar(int usuarioOperacionId,int id);

        incidenciaCompletaDTO? obtener(int id);

        List<idNombreDTO> listarEstados();
    }
    
}