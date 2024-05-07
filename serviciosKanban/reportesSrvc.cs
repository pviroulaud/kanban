using AutoMapper;
using entidadesKanban.modelo;
using serviciosKanban.DTO;
using serviciosKanban.SRVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace serviciosKanban
{
    public class reportesSrvc : IreportesSrvc
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Ilogger _log;
        private readonly ItareaSrvc _tareaSrvc;
        public reportesSrvc(AppDbContext context, IMapper mapper, Ilogger log)
        {
            _context = context;
            _mapper = mapper;
            _log = log;            
        }

        public reportePeriodoDTO horasPorDia(DateTime fechaDesde, DateTime fechaHasta, List<int> usuarioId, bool incluirEstadoCerrado)
        {
            reportePeriodoDTO res = new reportePeriodoDTO();
            res.fechaDesde = fechaDesde;
            res.fechaHasta = fechaHasta;
           

            var reg = (from r in _context.kbn_registroTiempo
                       where
                       ((usuarioId.Count == 0) ? (r.id > 0) : (usuarioId.Contains(r.usuarioId))) &&
                       r.fechaEjecucion >= fechaDesde &&
                       r.fechaEjecucion <= fechaHasta &&
                       (((incluirEstadoCerrado) ? (r.id > 0) : (r.estadoTareaId != 6)) ||
                       ((incluirEstadoCerrado) ? (r.id > 0) : (r.estadoTareaId != 6)))
                       select r).ToList(); 
            res.totalEjecucion = (from r in reg where r.estadoTareaId != 5 select r.ejecucion).Sum();
            res.totalBloqueo = (from r in reg where r.estadoTareaId == 5 select r.ejecucion ).Sum();
            res.totalEstimacion = (from t in _context.kbn_tarea where (from rg in reg select rg.tareaId).ToList().Contains(t.id) select t.estimacion).Sum();

            var grup=reg.GroupBy(ej => ej.fechaEjecucion).OrderBy(f => f.Key);

            List<reporteDiarioDTO> rta = new List<reporteDiarioDTO>();
            foreach (var item in grup)
            {
                reporteDiarioDTO r = new reporteDiarioDTO();
                r.fecha = item.Key;
                var lstTareasDia = (from i in item select i.tareaId).Distinct().ToList();

                r.totalEjecucion = (from tar in _context.kbn_registroTiempo where lstTareasDia.Contains(tar.tareaId) && tar.estadoTareaId != 5 select tar.ejecucion).Sum();
                r.totalBloqueo = (from tar in _context.kbn_registroTiempo where lstTareasDia.Contains(tar.tareaId) && tar.estadoTareaId == 5 select tar.ejecucion).Sum();


                var tareasDia= (from t in _context.kbn_tarea
                               join et in _context.kbn_estado on t.estadoId equals et.id
                               join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                               where
                               lstTareasDia.Contains(t.id)
                               select new tareaCompletaDTO()
                               {
                                   id = t.id,
                                   nombre = t.nombre,
                                   tipoTareaId = t.tipoTareaId,

                                   estadoId = t.estadoId,
                                   usuarioCreadorId = t.usuarioCreadorId,
                                   usuarioResponsableId = t.usuarioResponsableId,
                                   incidenciaId = t.incidenciaId,
                                   //fechaCreacion = r.fechaEjecucion,
                                   semanaDeEjecucionPlanificada = t.semanaDeEjecucionPlanificada,
                                   estimacion = t.estimacion,
                                   ejecucion = (from tar in _context.kbn_registroTiempo where tar.id == t.id && tar.estadoTareaId != 5 select tar.ejecucion).Sum(),
                                   bloqueo = (from tar in _context.kbn_registroTiempo where tar.tareaId == t.id && tar.estadoTareaId == 5 select tar.ejecucion).Sum(),
                                   //descripcion="",
                                   semanaDeEjecucionReal = t.semanaDeEjecucionReal,
                                   //nombreUsuarioResponsable = u.nombre,
                                   nombreIncidencia = i.nombre,
                                   nombreEstadoTarea = et.nombre

                               }).ToList();

                r.tareas.AddRange(tareasDia);
                rta.Add(r);
            }
            res.detalleDiario = rta;
            return res;

        }
    }
}
