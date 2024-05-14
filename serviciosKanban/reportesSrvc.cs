using AutoMapper;
using entidadesKanban.modelo;
using serviciosKanban.DTO;
using serviciosKanban.SRVC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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


        public List<reporteActividadDTO> reporteActividadIncidencia(int incidenciaId)
        {

            return (from r in _context.kbn_registroTiempo
                    join t in _context.kbn_tarea on r.tareaId equals t.id
                    join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                    join u in _context.kbn_usuario on r.usuarioId equals u.id
                    join e in _context.kbn_estado on r.estadoTareaId equals e.id
            where
                    t.incidenciaId == incidenciaId
                    select new reporteActividadDTO()
                    {
                        id = r.id,
                        incidenciaId = t.incidenciaId,
                        nombreIncidencia = i.nombre,
                        usuarioId = r.usuarioId,
                        nombreUsuario = u.nombre,
                        tareaId = t.id,
                        nombreTarea = t.nombre,
                        estadoTareaId = r.estadoTareaId,
                        nombreEstadoTarea = e.nombre,
                        ejecucion = r.ejecucion,
                        descripcion = r.descripcion,
                        fechaEjecucion = r.fechaEjecucion,
                        fechaRegistro = r.fechaRegistro
                    }).OrderByDescending(x=>x.fechaRegistro).ToList();
        }
        public List<reporteActividadDTO> reporteActividadTarea(int tareaId)
        {
            return (from r in _context.kbn_registroTiempo
                      join t in _context.kbn_tarea on r.tareaId equals t.id
                      join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                      join u in _context.kbn_usuario on r.usuarioId equals u.id
                      join e in _context.kbn_estado on r.estadoTareaId equals e.id
                      where
                      t.id == tareaId
                      select new reporteActividadDTO()
                      {
                          id=r.id,
                          incidenciaId=t.incidenciaId,
                          nombreIncidencia=i.nombre,
                          usuarioId =r.usuarioId,
                          nombreUsuario = u.nombre,
                          tareaId=t.id,
                          nombreTarea=t.nombre,
                          estadoTareaId=r.estadoTareaId,
                          nombreEstadoTarea = e.nombre,
                          ejecucion=r.ejecucion,
                          descripcion=r.descripcion,
                          fechaEjecucion=r.fechaEjecucion,
                          fechaRegistro=r.fechaRegistro                          
                      }).OrderByDescending(x => x.fechaRegistro).ToList();
        }
        public reporteSemanaDTO planificacionSemana(string semana,List<int> usuarioId)
        {
            reporteSemanaDTO ret = new reporteSemanaDTO();
           
            int sem = Convert.ToInt32(semana.Replace("-W", ""));

            var regSemana = (from t in _context.kbn_tarea
                                join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                                where
                                ((usuarioId.Count == 0 || t.usuarioResponsableId == null) ? (t.id > 0) : (usuarioId.Contains(t.usuarioResponsableId ?? 0))) &&
                                t.semanaDeEjecucionPlanificada == sem
                                select new detalleReporteSemanaDTO()
                                {
                                     
                                    incidenciaId = t.incidenciaId,
                                    nombreIncidencia = i.nombre,
                                    tareaId = t.id,
                                    nombreTarea = t.nombre,
                                    usuarioResponsableId = t.usuarioResponsableId,
                                    nombreUsuarioResponsable = ((t.usuarioResponsableId == null) ? ("-") : (from u in _context.kbn_usuario where u.id == t.usuarioResponsableId select u.nombre).FirstOrDefault() ?? "-"),
                                    estimacion = t.estimacion

                                }).ToList();
            ret.detalle.AddRange(regSemana);

            string[] yyyyWss = semana.Split("-W", StringSplitOptions.RemoveEmptyEntries);

            ret.semanaEjecucionPlanificada = yyyyWss[1]+"/"+ yyyyWss[0];
            ret.estimacionTotal = (from t in regSemana select t.estimacion).Sum();

            
            ret.fechaDesde = ISOWeek.ToDateTime(Convert.ToInt32(yyyyWss[0]), Convert.ToInt32(yyyyWss[1]), DayOfWeek.Monday);
            ret.fechaHasta = ISOWeek.ToDateTime(Convert.ToInt32(yyyyWss[0]), Convert.ToInt32(yyyyWss[1]), DayOfWeek.Sunday);

            return ret;
        }

        public List<reporteSemanaDTO> planificacionSemana(string semana,string semanaHasta, List<int> usuarioId)
        {
            List<reporteSemanaDTO> ret = new List<reporteSemanaDTO>();

            int sem = Convert.ToInt32(semana.Replace("-W", ""));
            int semHasta = Convert.ToInt32(semanaHasta.Replace("-W", ""));

            List<int?> semanas = (from t in _context.kbn_tarea
                           where
                           t.semanaDeEjecucionPlanificada >= sem &&
                           t.semanaDeEjecucionPlanificada <= semHasta
                           select t.semanaDeEjecucionPlanificada).Distinct().ToList();

        

            foreach (var item in semanas)
            {
                reporteSemanaDTO reporteSem = new reporteSemanaDTO();

                var regSemana = (from t in _context.kbn_tarea
                                 join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                                 where
                                 ((usuarioId.Count == 0 || t.usuarioResponsableId == null) ? (t.id > 0) : (usuarioId.Contains(t.usuarioResponsableId ?? 0))) &&
                                 t.semanaDeEjecucionPlanificada == item
                                 select new detalleReporteSemanaDTO()
                                 {

                                     incidenciaId = t.incidenciaId,
                                     nombreIncidencia = i.nombre,
                                     tareaId = t.id,
                                     nombreTarea = t.nombre,
                                     usuarioResponsableId = t.usuarioResponsableId,
                                     nombreUsuarioResponsable = ((t.usuarioResponsableId == null) ? ("-") : (from u in _context.kbn_usuario where u.id == t.usuarioResponsableId select u.nombre).FirstOrDefault() ?? "-"),
                                     estimacion = t.estimacion

                                 }).ToList();
                reporteSem.detalle.AddRange(regSemana);
                

                reporteSem.semanaEjecucionPlanificada = item.ToString().Substring(0,4) + "/" + item.ToString().Substring(4,2);
                reporteSem.estimacionTotal = (from t in regSemana select t.estimacion).Sum();

                string[] yyyyWss = semana.Split("-W", StringSplitOptions.RemoveEmptyEntries);
                reporteSem.fechaDesde = ISOWeek.ToDateTime(Convert.ToInt32(yyyyWss[0]), Convert.ToInt32(yyyyWss[1]), DayOfWeek.Monday);
                yyyyWss = semanaHasta.Split("-W", StringSplitOptions.RemoveEmptyEntries);
                reporteSem.fechaHasta = ISOWeek.ToDateTime(Convert.ToInt32(yyyyWss[0]), Convert.ToInt32(yyyyWss[1]), DayOfWeek.Sunday);

                ret.Add(reporteSem);
            }



            return ret;
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
            res.totalEjecucion = (from r in reg 
                                  where
                                  ((usuarioId.Count == 0) ? (r.id > 0) : (usuarioId.Contains(r.usuarioId))) && 
                                  r.estadoTareaId != 5 select r.ejecucion).Sum();
            res.totalBloqueo = (from r in reg 
                                where
                                ((usuarioId.Count == 0) ? (r.id > 0) : (usuarioId.Contains(r.usuarioId))) && 
                                r.estadoTareaId == 5 select r.ejecucion ).Sum();
            res.totalEstimacion = (from t in _context.kbn_tarea where (from rg in reg select rg.tareaId).ToList().Contains(t.id) select t.estimacion).Sum();

            var grup=reg.GroupBy(ej => ej.fechaEjecucion).OrderBy(f => f.Key);

            List<reporteDiarioDTO> rta = new List<reporteDiarioDTO>();
            foreach (var item in grup)
            {
                reporteDiarioDTO r = new reporteDiarioDTO();
                r.fecha = item.Key;
                var lstTareasDia = (from i in item select i.id).Distinct().ToList();

                r.totalEjecucion = (from tar in _context.kbn_registroTiempo where lstTareasDia.Contains(tar.id) && tar.estadoTareaId != 5 select tar.ejecucion).Sum();
                r.totalBloqueo = (from tar in _context.kbn_registroTiempo where lstTareasDia.Contains(tar.id) && tar.estadoTareaId == 5 select tar.ejecucion).Sum();


                var tareasDia= (from t in _context.kbn_tarea
                               join et in _context.kbn_estado on t.estadoId equals et.id
                               join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                               join rt in _context.kbn_registroTiempo on t.id equals rt.tareaId
                               where
                               lstTareasDia.Contains(rt.id)
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
                                   ejecucion = (from tar in _context.kbn_registroTiempo where tar.id == rt.id && tar.estadoTareaId != 5 select tar.ejecucion).Sum(),
                                   bloqueo = (from tar in _context.kbn_registroTiempo where tar.id == rt.id && tar.estadoTareaId == 5 select tar.ejecucion).Sum(),
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
