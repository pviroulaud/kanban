using serviciosKanban.DTO;
using System.Text.Json;
using serviciosKanban.SRVC;
using entidadesKanban.modelo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using System.Security.Cryptography;

namespace serviciosKanban.SRVC
{
    public class tareaSrvc:ItareaSrvc
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Ilogger _log;
        public tareaSrvc(AppDbContext context,IMapper mapper,Ilogger log)
        {
            _context=context;
            _mapper=mapper;
            _log=log;
        }

        public List<idNombreDTO> listarTipos()
        {
            var lst = from ti in _context.kbn_tipoTarea select new idNombreDTO(){
                id=ti.id,
                nombre=ti.nombre
            };
            return lst.ToList();
        }

        public List<tareaDTO> listar(int incidenciaId)
        {

            return _mapper.Map<List<tareaDTO>>(_context.kbn_tarea.Include(tp => tp.tipoTarea).Where(i => i.incidenciaId == incidenciaId));

        }
         public List<tareaCompletaDTO> listar(filtroBusquedaDTO filtros)
        {
            var lst = (from t in _context.kbn_tarea
                       join tt in _context.kbn_tipoTarea on t.tipoTareaId equals tt.id
                       join et in _context.kbn_estado on t.estadoId equals et.id
                       join i in _context.kbn_incidencia on t.incidenciaId equals i.id
                       join p in _context.kbn_proyecto on i.proyectoId equals p.id
                       join u in _context.kbn_usuario on t.usuarioResponsableId equals u.id into tareaAsignada
                       from lstTarea in tareaAsignada.DefaultIfEmpty()
                       where

                       ((filtros.tipoTareaId.Count == 0) ? (t.id > 0) : (filtros.tipoTareaId.Contains(t.tipoTareaId.ToString()))) &&
                       ((filtros.estadoTareaId.Count == 0) ? (t.id > 0) : (filtros.estadoTareaId.Contains(t.estadoId.ToString()))) &&
                       ((filtros.incidenciaId.Count == 0) ? (t.id > 0) : (filtros.incidenciaId.Contains(t.incidenciaId.ToString()))) &&
                       ((filtros.proyectoId.Count == 0) ? (t.id > 0) : (filtros.proyectoId.Contains(i.proyectoId.ToString()))) &&
                       ((filtros.usuarioResponsableId.Count == 0) ? (t.id > 0) : (filtros.usuarioResponsableId.Contains(t.usuarioResponsableId.ToString())))

                       select new tareaCompletaDTO()
                       {
                           id = t.id,
                           nombre = t.nombre,
                           tipoTareaId = t.tipoTareaId,
                           nombreTarea = tt.nombre,
                           estadoId = t.estadoId,
                           usuarioCreadorId=t.usuarioCreadorId,
                           usuarioResponsableId=t.usuarioResponsableId,
                           incidenciaId=t.incidenciaId,
                           fechaCreacion=t.fechaCreacion,
                           semanaDeEjecucionPlanificada=t.semanaDeEjecucionPlanificada,
                           estimacion=t.estimacion,
                           ejecucion=t.ejecucion,
                           //descripcion="",
                           semanaDeEjecucionReal=t.semanaDeEjecucionReal,
                           nombreUsuarioResponsable= lstTarea.nombre,
                           nombreIncidencia=i.nombre,
                           nombreProyecto=p.nombre,
                           nombreEstadoTarea=et.nombre

                       }).ToList();



            return lst;

        }
        public List<tareaDTO> listar(int incidenciaId,filtroBusquedaDTO filtros)
        {
            return _mapper.Map<List<tareaDTO>>(from t in _context.kbn_tarea where t.incidenciaId ==incidenciaId select t);
        }
        public tareaDTO? obtener(int id)
        {
            var t = (from ta in _context.kbn_tarea where ta.id==id select ta).FirstOrDefault();
            if (t==null)
            {
                return null;
            }
            
            var tDTO= _mapper.Map<tareaDTO>(t);
            tDTO.proyectoId = (from p in _context.kbn_incidencia where p.id == t.incidenciaId select p.proyectoId).FirstOrDefault();
            return tDTO;
        }

        public int nuevo(int usuarioOperacionId,nuevaTareaDTO datos)
        {
            if (datos.nombre.Trim()!="")
            {
                var t = _mapper.Map<kbn_tarea>(datos);
                t.usuarioCreadorId = usuarioOperacionId;
                t.fechaCreacion = DateTime.Now;
                if (t.usuarioResponsableId == 0) t.usuarioResponsableId = null;
                _context.kbn_tarea.Add(t);
                _context.SaveChanges();

                _log.registrarAlta(usuarioOperacionId,"tarea",t.id,JsonSerializer.Serialize(datos));
                return t.id;
            }
            else{
                return 0;
            }
        }
        public int actualizar(int usuarioOperacionId,tareaDTO datos)
        {
            var t = (from ta in _context.kbn_tarea where ta.id==datos.id select ta).FirstOrDefault();
            if (t==null)
            {
                return -1;
            }
            if (datos.nombre.Trim()!="")
            {
                t.nombre = datos.nombre;
                t.tipoTareaId = datos.tipoTareaId;
                t.descripcion = datos.descripcion;
                t.estadoId = datos.estadoId;
                t.usuarioResponsableId = (datos.usuarioResponsableId == 0) ? null : datos.usuarioResponsableId;
                t.ejecucion = datos.ejecucion;
                t.estimacion= datos.estimacion;
                t.semanaDeEjecucionPlanificada = datos.semanaDeEjecucionPlanificada;
                t.semanaDeEjecucionReal = datos.semanaDeEjecucionReal;

                //_mapper.Map(datos,t);
                _context.kbn_tarea.Update(t);
                _context.SaveChanges();

                _log.registrarModificacion(usuarioOperacionId,"tarea",t.id,JsonSerializer.Serialize(datos));
                return t.id;
            }
            else{
                return 0;
            }
        }

        public void cambiarEstadoTarea(int usuarioOperacionId,int tareaId,int estadoId)
        {
            var t = (from ta in _context.kbn_tarea where ta.id == tareaId && ta.estadoId!=estadoId select ta).FirstOrDefault();
            if (t != null)
            {
                 t.estadoId=estadoId;
                _context.kbn_tarea.Update(t);
                _context.SaveChanges();

                _log.registrarModificacion(usuarioOperacionId, "tarea", t.id, "{'id':"+t.id+",'estadoId':"+t.estadoId+"}");
            }
        }
        public int registrarTiempo(int usuarioOperacionId, registroTiempoDTO datos)
        {

            var t = _mapper.Map<kbn_registroTiempo>(datos);
            t.usuarioId = usuarioOperacionId;    
            t.fechaRegistro = DateTime.Now;
                
            _context.kbn_registroTiempo.Add(t);
            _context.SaveChanges();

            var tarea = (from ta in _context.kbn_tarea where ta.id == datos.tareaId select ta).FirstOrDefault();
            if (tarea !=null)
            {
                tarea.ejecucion += datos.ejecucion;
                _context.kbn_tarea.Update(tarea); 
                _context.SaveChanges();
            }

            _log.registrarAlta(usuarioOperacionId, "registroTiempo", t.id, JsonSerializer.Serialize(datos));
            return t.id;
        }
        public bool eliminar(int usuarioOperacionId,int id)
        {
            var t = (from ta in _context.kbn_tarea where ta.id==id select ta).FirstOrDefault();
            if (t==null)
            {
                return false;
            }
            _context.kbn_tarea.Remove(t);
            _context.SaveChanges();
            _log.registrarBaja(usuarioOperacionId,"tarea",t.id);
            return true;
        }
    }
}
