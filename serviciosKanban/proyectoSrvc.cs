using serviciosKanban.DTO;
using System.Text.Json;
using entidadesKanban.modelo;
using AutoMapper;
namespace serviciosKanban.SRVC
{
    public class proyectoSrvc:IproyectoSrvc
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Ilogger _log;
        public proyectoSrvc(AppDbContext context,Ilogger log,IMapper mapper)
        {
            _context=context;
            _mapper=mapper;
            _log=log;
        }

        public List<proyectoDTO> listar()
        {
            return _mapper.Map<List<proyectoDTO>>(from p in _context.kbn_proyecto where p.id >0 select p);
        }

        public proyectoDTO? obtener(int id)
        {
            var p = (from py in _context.kbn_proyecto where py.id==id select py).FirstOrDefault();
            if (p==null)
            {
                return null;
            }
            
            return _mapper.Map<proyectoDTO>(p);
        }
        public int nuevo(int usuarioOperacionId,nuevoProyectoDTO datos)
        {
            if (datos.nombre.Trim()!="")
            {
                var p = _mapper.Map<kbn_proyecto>(datos);
                _context.kbn_proyecto.Add(p);
                _context.SaveChanges();
                _log.registrarAlta(usuarioOperacionId,"proyecto",p.id,JsonSerializer.Serialize(datos));

                return p.id;
            }
            else{
                return 0;
            }
        }
        public int actualizar(int usuarioOperacionId,proyectoDTO datos)
        {
            kbn_proyecto p = (from py in _context.kbn_proyecto where py.id==datos.id select py).FirstOrDefault();
            if (p==null)
            {
                return -1;
            }
            if (datos.nombre.Trim()!="")
            {
                _mapper.Map(datos,p);                
                _context.kbn_proyecto.Update(p);
                _context.SaveChanges();
                _log.registrarModificacion(usuarioOperacionId,"proyecto",p.id,JsonSerializer.Serialize(datos));

                return p.id;
            }
            else{
                return 0;
            }
        }

        public bool eliminar(int usuarioOperacionId,int id)
        {
            var p = (from py in _context.kbn_proyecto where py.id==id select py).FirstOrDefault();
            if (p==null)
            {
                return false;
            }
            _context.kbn_proyecto.Remove(p);
            _context.SaveChanges();
            _log.registrarBaja(usuarioOperacionId,"proyecto",p.id);
            return true;
        }
    }
}
