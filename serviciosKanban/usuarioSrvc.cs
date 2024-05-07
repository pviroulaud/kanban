using serviciosKanban.DTO;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using entidadesKanban.modelo;
using AutoMapper;
namespace serviciosKanban.SRVC
{
    public class usuarioSrvc:IusuarioSrvc
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Ilogger _log;
        private readonly IConfiguration _configuration;
        private readonly Ijwt _jwt;
        public usuarioSrvc(AppDbContext context,IMapper mapper,Ijwt jwt,IConfiguration configuration,Ilogger log)
        {
            _context=context;
            _mapper=mapper;
            _log=log;
            _configuration=configuration;
            _jwt=jwt;
        }

        public respuestaLoginDTO autenticar(loginDTO credenciales)
        {
            respuestaLoginDTO rta= new respuestaLoginDTO();
            var u = (from us in _context.kbn_usuario 
            join usP in _context.kbn_usuarioPassword on us.id equals usP.usuarioId
            where us.nombre== credenciales.usuario && usP.pass==credenciales.pass
            select us).FirstOrDefault();

            if (u != null)
            {
                rta.autorizado=true;
                rta.usuario=u.nombre;

                //Generar Token
                Dictionary<string, string> claims= new Dictionary<string, string>();
                claims.Add("Id", u.id.ToString());

                rta.tkn=_jwt.GenerarTokenJWT(claims,
                        u.id,
                        u.nombre+"@"+u.correo,
                        _configuration,
                        8,0);
            }
            return rta;
        }
        
        public List<usuarioDTO> listar()
        {
            return _mapper.Map<List<usuarioDTO>>(from u in _context.kbn_usuario where u.id >0 select u);
        }

        public usuarioDTO? obtener(int id)
        {
            var u = (from us in _context.kbn_usuario where us.id==id select us).FirstOrDefault();
            if (u==null)
            {
                return null;
            }
            
            return _mapper.Map<usuarioDTO>(u);
        }

        public int nuevo(int usuarioOperacionId,nuevoUsuarioDTO datos)
        {
            if ((datos.nombre.Trim()!="") && (datos.correo.Trim()!=""))
            {
                var u = _mapper.Map<kbn_usuario>(datos);
                _context.kbn_usuario.Add(u);
                _context.SaveChanges();

                var pwd = new kbn_usuarioPassword()
                {
                    usuarioId = u.id,
                    pass = u.nombre.Trim() + "01*"
                };
                _context.kbn_usuarioPassword.Add(pwd);
                _context.SaveChanges();

                _log.registrarAlta(usuarioOperacionId,"usuario",u.id,JsonSerializer.Serialize(datos));
                return u.id;
            }
            else{
                return 0;
            }
        }
        public int actualizar(int usuarioOperacionId,usuarioDTO datos)
        {
            var u = (from us in _context.kbn_usuario where us.id==datos.id select us).FirstOrDefault();
            if (u==null)
            {
                return -1;
            }
            if ((datos.nombre.Trim()!="") && (datos.correo.Trim()!=""))
            {
                _mapper.Map(datos,u);
                _context.kbn_usuario.Update(u);
                _context.SaveChanges();

                _log.registrarModificacion(usuarioOperacionId,"usuario",u.id,JsonSerializer.Serialize(datos));
                return u.id;
            }
            else{
                return 0;
            }
        }

        public bool eliminar(int usuarioOperacionId,int id)
        {
            var u = (from us in _context.kbn_usuario where us.id==id select us).FirstOrDefault();
            if (u==null)
            {
                return false;
            }
            _context.kbn_usuario.Remove(u);
            _context.SaveChanges();
            _log.registrarBaja(usuarioOperacionId,"usuario",u.id);
            return true;
        }
    }
}
