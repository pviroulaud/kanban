using serviciosKanban.DTO;
using serviciosKanban.SRVC;
using System.Text.Json;

using entidadesKanban.modelo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.CodeAnalysis;

namespace serviciosKanban.SRVC
{
    public class comparadorIncidenciaCompleta : IEqualityComparer<incidenciaCompletaDTO>
    {
        public bool Equals(incidenciaCompletaDTO? x, incidenciaCompletaDTO? y)
        {
            return x.id==y.id;
        }

        public int GetHashCode([DisallowNull] incidenciaCompletaDTO obj)
        {
            return obj.id.GetHashCode();
        }
    }
    public class incidenciaSrvc:IincidenciaSrvc
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Ilogger _log;
        private readonly ItareaSrvc _tareaSrvc;
        public incidenciaSrvc(AppDbContext context,IMapper mapper,Ilogger log,ItareaSrvc tareaSrvc)
        {
            _context=context;
            _mapper=mapper;
            _log=log;
            _tareaSrvc=tareaSrvc;
        }
        public List<idNombreDTO> listarTipos()
        {
            var lst = from ti in _context.kbn_tipoIncidencia select new idNombreDTO(){
                id=ti.id,
                nombre=ti.nombre
            };
            return lst.ToList();
        }

        public List<idNombreDTO> listarEstados()
        {
            var lst = from ti in _context.kbn_estado select new idNombreDTO(){
                id=ti.id,
                nombre=ti.nombre
            };
            return lst.ToList();
        }

        public List<idNombreDTO> listarNombreIncidenciasCerradas(int proyectoId)
        {
            return _mapper.Map<List<idNombreDTO>>(from i in _context.kbn_incidencia
                                                  where
                                                  i.proyectoId == proyectoId &&
                                                   i.estadoId == 6

                                                  select i);
        }
        public List<idNombreDTO> listarNombreIncidenciasCerradas()
        {
            return _mapper.Map<List<idNombreDTO>>(from i in _context.kbn_incidencia
                                                  where
                                                  i.estadoId == 6
                                                  select i);
        }

        public List<idNombreDTO> listarNombreIncidencias(int proyectoId,bool incluirCerradas=false)
        {
            return _mapper.Map<List<idNombreDTO>>(from i in _context.kbn_incidencia 
                                                  where 
                                                  i.proyectoId==proyectoId  &&
                                                  (incluirCerradas? i.id>0 : i.estadoId!=6)
                                                  
                                                  select i);
        }
        public List<idNombreDTO> listarNombreIncidencias(bool incluirCerradas = false)
        {
            return _mapper.Map<List<idNombreDTO>>(from i in _context.kbn_incidencia 
                                                  where
                                                  (incluirCerradas ? i.id > 0 : i.estadoId != 6)
                                                  select i);
        }
        public List<incidenciaCompletaDTO> listar()
        {
            var lst= (from i in _context.kbn_incidencia
                     join ti in _context.kbn_tipoIncidencia on i.tipoIncidenciaId equals ti.id
                     join ei in _context.kbn_estado on i.estadoId equals ei.id
                     join p in _context.kbn_proyecto on i.proyectoId equals p.id
                     select new incidenciaCompletaDTO(){
                        id=i.id,
                        nombre=i.nombre,
                        tipoIncidenciaId=i.tipoIncidenciaId,
                        estadoId=i.estadoId,
                        usuarioCreadorId=i.usuarioCreadorId,
                        nombreCreador= i.usuarioCreador.nombre,
                        usuarioResponsableId=i.usuarioResponsableId,
                        descripcion=i.descripcion,

                        fechaCreacion =i.fechaCreacion,
                        proyectoId=i.proyectoId,

                        nombreProyecto=p.nombre,
                        codigoProyecto=p.codigoProyecto,

                        nombreEstado=ei.nombre,
                        nombreTipoIncidencia=ti.nombre,

                        estimacionTotal=(from t in _context.kbn_tarea where t.incidenciaId == i.id select t.estimacion).Sum(),
                        ejecucionTotal=(from t in _context.kbn_tarea where t.incidenciaId == i.id select t.ejecucion).Sum(),
                         resumenTareas = obtenerResumenDeTareasPorTipo(_mapper.Map<List<tareaDTO>>(_context.kbn_tarea.Include(tp => tp.tipoTarea).Where(inc => inc.incidenciaId == i.id)),null)
                     }).ToList();

            return lst;
        }

        public List<incidenciaCompletaDTO> listar(filtroBusquedaDTO filtros, bool incluirCerradas = false)
        {

            var lst = (from i in _context.kbn_incidencia
                       join ti in _context.kbn_tipoIncidencia on i.tipoIncidenciaId equals ti.id
                       join ei in _context.kbn_estado on i.estadoId equals ei.id
                       join p in _context.kbn_proyecto on i.proyectoId equals p.id
                       join t in _context.kbn_tarea on i.id equals t.incidenciaId into incid
                       from inciden in incid.DefaultIfEmpty()
                       where
                       (incluirCerradas ? i.id > 0 : i.estadoId != 6) &&
                       ((filtros.estadoIncidenciaId.Count == 0) ? (i.id > 0) : (filtros.estadoIncidenciaId.Contains(i.estadoId.ToString()))) &&
                       ((filtros.proyectoId.Count == 0) ? (i.id > 0) : (filtros.proyectoId.Contains( i.proyectoId.ToString()))) &&
                       ((filtros.tipoIncidenciaId.Count == 0) ? (i.id > 0) : (filtros.tipoIncidenciaId.Contains(i.tipoIncidenciaId.ToString()))) &&
                       ((filtros.usuarioCreadorId.Count == 0) ? (i.id > 0) : (filtros.usuarioCreadorId.Contains(i.usuarioCreadorId.ToString()))) &&
                       ((filtros.usuarioResponsableId.Count == 0) ? (i.id > 0) : (filtros.usuarioResponsableId.Contains(i.usuarioResponsableId.ToString())) ||
                                                                                 (filtros.usuarioResponsableId.Contains(inciden.usuarioResponsableId.ToString())))
                       select new incidenciaCompletaDTO()
                       {
                           id = i.id,
                           nombre = i.nombre,
                           tipoIncidenciaId = i.tipoIncidenciaId,
                           estadoId = i.estadoId,
                           usuarioCreadorId = i.usuarioCreadorId,
                           nombreCreador = i.usuarioCreador.nombre,
                           usuarioResponsableId = i.usuarioResponsableId,
                           descripcion = i.descripcion,

                           fechaCreacion = i.fechaCreacion,
                           proyectoId = i.proyectoId,

                           nombreProyecto = p.nombre,
                           codigoProyecto = p.codigoProyecto,

                           nombreEstado = ei.nombre,
                           nombreTipoIncidencia = ti.nombre,

                           estimacionTotal = (from t in _context.kbn_tarea where t.incidenciaId == i.id select t.estimacion).Sum(),
                           ejecucionTotal = (from t in _context.kbn_tarea where t.incidenciaId == i.id select t.ejecucion).Sum(),
                           resumenTareas = obtenerResumenDeTareasPorTipo(_mapper.Map<List<tareaDTO>>(_context.kbn_tarea.Include(tp => tp.tipoTarea).Where(inc => inc.incidenciaId == i.id)),filtros.usuarioResponsableId)
                       }).ToList();
           
            return lst.DistinctBy(x => x.id).ToList();
        }

        public incidenciaCompletaDTO? obtener(int id)
        {
            var i = (from inc in _context.kbn_incidencia where inc.id==id select inc).FirstOrDefault();
            if (i==null)
            {
                return null;
            }
            
            var inCompl= _mapper.Map<incidenciaCompletaDTO>(i);            
            inCompl.tareas=_mapper.Map<List<tareaDTO>>(_tareaSrvc.listar(i.id));

            var py= (from p in _context.kbn_proyecto where p.id==i.proyectoId select p).FirstOrDefault();
            inCompl.nombreProyecto=py.nombre;
            inCompl.codigoProyecto=py.codigoProyecto;

            inCompl.nombreEstado= (from e in _context.kbn_estado where e.id== i.estadoId select e.nombre).FirstOrDefault()??"-";
            inCompl.nombreTipoIncidencia=(from ti in _context.kbn_tipoIncidencia where ti.id== i.tipoIncidenciaId select ti.nombre).FirstOrDefault()??"-"; 
            inCompl.nombreCreador= (from u in _context.kbn_usuario where u.id== i.usuarioCreadorId select u.nombre).FirstOrDefault()??"-";  

            inCompl.estimacionTotal=(from t in _context.kbn_tarea where t.incidenciaId == i.id select t.estimacion).Sum();
            inCompl.ejecucionTotal=(from t in _context.kbn_tarea where t.incidenciaId == i.id select t.ejecucion).Sum();                   

            inCompl.resumenTareas= obtenerResumenDeTareasPorTipo(inCompl.tareas);
            
            // Verificar! Hay que agrupar las tareas por tipo y armar el inCompl.resumenTareas
            

            return inCompl;
        }

        private static List<resumenPorTipoTareaDTO> obtenerResumenDeTareasPorTipo(List<tareaDTO> tareas,List<string>usuarioResponsableId = null)
        {
            List<resumenPorTipoTareaDTO> resumen = new List<resumenPorTipoTareaDTO>();

            var rt = (from t in tareas 
                      where 
                      ((usuarioResponsableId==null || usuarioResponsableId.Count==0)?(t.id>0):( usuarioResponsableId.Contains(t.usuarioResponsableId.ToString()) ))
                      select t).GroupBy(x => x.tipoTareaId).ToList();
            foreach (var item in rt)
            {
                resumen.Add(new resumenPorTipoTareaDTO()
                {
                    tipoTareaId = item.Key,
                    nombreTarea = item.FirstOrDefault().nombreTarea,
                    cantidadTareas = item.Count(),
                    estimacion = item.ToList().Sum(x => x.estimacion) ?? 0,
                    ejecucion = item.ToList().Sum(x => x.ejecucion) ?? 0
                });
            }
            return resumen;
        }
        public int nuevo(int usuarioOperacionId,nuevaIncidenciaDTO datos)
        {
            if (datos.nombre.Trim()!="")
            {
                var i = _mapper.Map<kbn_incidencia>(datos);
                i.usuarioCreadorId=usuarioOperacionId;
                i.usuarioResponsableId=usuarioOperacionId;
                i.fechaCreacion=DateTime.Now;
                i.estadoId=1;

                _context.kbn_incidencia.Add(i);
                _context.SaveChanges();

                _log.registrarAlta(usuarioOperacionId,"incidencia",i.id,JsonSerializer.Serialize(datos));
                return i.id;
            }
            else{
                return 0;
            }
        }
        public int actualizar(int usuarioOperacionId,incidenciaDTO datos)
        {
            var i = (from inc in _context.kbn_incidencia where inc.id==datos.id select inc).FirstOrDefault();
            if (i==null)
            {
                return -1;
            }
            if (datos.nombre.Trim()!="") 
            {
                i.proyectoId = datos.proyectoId;
                i.nombre = datos.nombre;
                i.tipoIncidenciaId = datos.tipoIncidenciaId;
                i.descripcion= datos.descripcion;
                i.estadoId=datos.estadoId;
                i.usuarioResponsableId = datos.usuarioResponsableId;
                
                _context.kbn_incidencia.Update(i);
                _context.SaveChanges();

                if (i.estadoId==6)
                {
                    var tareas = _tareaSrvc.listar(i.id);
                    foreach (var item in tareas)
                    {
                        _tareaSrvc.cambiarEstadoTarea(usuarioOperacionId, item.id, i.estadoId);
                    }
                }
                _log.registrarModificacion(usuarioOperacionId,"incidencia",i.id,JsonSerializer.Serialize(datos));
                return i.id;
            }
            else{
                return 0;
            }
        }

        public bool eliminar(int usuarioOperacionId,int id)
        {
            var i = (from inc in _context.kbn_incidencia where inc.id==id select inc).FirstOrDefault();
            if (i==null)
            {
                return false;
            }
            _context.kbn_incidencia.Remove(i);
            _context.SaveChanges();
            _log.registrarBaja(usuarioOperacionId,"incidencia",i.id);
            return true;
        }


    }
}
