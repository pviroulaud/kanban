using serviciosKanban.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serviciosKanban.SRVC
{
    public interface IregistroTiempo
    {
        void crearRegistro(registroTiempoDTO datos);

        List<registroTiempoDTO> obtenerRegistroTiempo(int tareaId);
        List<registroTiempoDTO> obtenerRegistroTiempo(int tareaId,TimeSpan rangoFecha);
        List<registroTiempoDTO> obtenerRegistroTiempo(int tareaId,int estadoId);
        List<registroTiempoDTO> obtenerRegistroTiempo(int tareaId, int estadoId, TimeSpan rangoFecha);
        decimal obtenerTiempoTotal(int tareaId);
        decimal obtenerTiempoTotal(int tareaId, TimeSpan rangoFecha);
        decimal obtenerTiempoTotal(int tareaId, int estadoId);
        decimal obtenerTiempoTotal(int tareaId, int estadoId, TimeSpan rangoFecha);
    }
}
