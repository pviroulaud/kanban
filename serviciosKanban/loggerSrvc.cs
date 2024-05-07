using serviciosKanban.DTO;
using entidadesKanban.modelo;
using AutoMapper;
namespace serviciosKanban.SRVC
{
    public class loggerSrvc:Ilogger
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public loggerSrvc(AppDbContext context,IMapper mapper)
        {
            _context=context;
            _mapper=mapper;
        }

        public void registrarAlta(int usuarioId,string entidad,int entidadId=0,string? detalle= null)
        {
            kbn_log l= new kbn_log()
            {
                usuarioId=usuarioId,
                entidad=entidad.ToUpper(),
                entidadId=entidadId,
                detalles=detalle,
                fechaHora=DateTime.Now,
                accion="A",
            };
            _context.kbn_log.Add(l);
            _context.SaveChanges();

        }
        public void registrarBaja(int usuarioId,string entidad,int entidadId=0,string? detalle= null)
        {
            kbn_log l = new kbn_log()
            {
                usuarioId=usuarioId,
                entidad=entidad.ToUpper(),
                entidadId=entidadId,
                detalles=detalle,
                fechaHora=DateTime.Now,
                accion="B",
            };
            _context.kbn_log.Add(l);
            _context.SaveChanges();
        }
        public void registrarModificacion(int usuarioId,string entidad,int entidadId=0,string? detalle= null)
        {
            kbn_log l = new kbn_log()
            {
                usuarioId=usuarioId,
                entidad=entidad.ToUpper(),
                entidadId=entidadId,
                detalles=detalle,
                fechaHora=DateTime.Now,
                accion="M",
            };
            _context.kbn_log.Add(l);
            _context.SaveChanges();           
        }
    }
}
