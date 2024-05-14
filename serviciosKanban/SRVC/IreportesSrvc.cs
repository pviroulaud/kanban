using serviciosKanban.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serviciosKanban.SRVC
{
    public interface IreportesSrvc
    {
        List<reporteActividadDTO> reporteActividadIncidencia(int incidenciaId);
        List<reporteActividadDTO> reporteActividadTarea(int tareaId);
        reporteSemanaDTO planificacionSemana(string semana, List<int> usuarioId);
        List<reporteSemanaDTO> planificacionSemana(string semana, string semanaHasta, List<int> usuarioId);
        reportePeriodoDTO horasPorDia(DateTime fechaDesde,DateTime fechaHasta,List<int> usuarioId, bool incluirEstadoCerrado);
    }
}
