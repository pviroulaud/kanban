using serviciosKanban.DTO;
using System.Collections.Generic;


namespace serviciosKanban.SRVC
{
    public interface ItareaSrvc
    {
        List<idNombreDTO> listarTipos();
        
        List<tareaCompletaDTO> listar(filtroBusquedaDTO filtros);
        List<tareaDTO> listar(int incidenciaId);
        List<tareaDTO> listar(int incidenciaId,filtroBusquedaDTO filtros);
        int nuevo(int usuarioOperacionId,nuevaTareaDTO datos);
        int actualizar(int usuarioOperacionId,tareaDTO datos);
        int registrarTiempo(int usuarioOperacionId, registroTiempoDTO datos);
        bool eliminar(int usuarioOperacionId,int id);

        void cambiarEstadoTarea(int usuarioOperacionId, int tareaId, int estadoId);
        tareaDTO? obtener(int id);
    }
    
}