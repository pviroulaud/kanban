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
        reporteSemanaDTO planificacionSemana(string semana, List<int> usuarioId);
        reportePeriodoDTO horasPorDia(DateTime fechaDesde,DateTime fechaHasta,List<int> usuarioId, bool incluirEstadoCerrado);
    }
}
