using AutoMapper;
using serviciosKanban.DTO;
using entidadesKanban.modelo;

namespace serviciosKanban.profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // CreateMap<IGrouping<int, tareaDTO>,resumenTareaDTO>()
            // .ForMember(x=>x.tareaId, opt=>opt.MapFrom(s=>s.Key))
            // .ForMember(x=>x.tarea, opt=>opt.MapFrom(s=>s.Value));

            CreateMap<nuevoUsuarioDTO, kbn_usuario>();
            CreateMap<usuarioDTO, kbn_usuario>().ForMember(x => x.id, opt => opt.Ignore()); // ignorar el id para hacer los update
            CreateMap<kbn_usuario, usuarioDTO>();

            CreateMap<nuevoProyectoDTO, kbn_proyecto>();
            CreateMap<proyectoDTO, kbn_proyecto>().ForMember(x => x.id, opt=>opt.MapFrom(s=>s.id)); // ignorar el id para hacer los update
            CreateMap<kbn_proyecto, proyectoDTO>();

            CreateMap<nuevaIncidenciaDTO, kbn_incidencia>();
            CreateMap<incidenciaDTO, kbn_incidencia>().ForMember(x => x.id, opt => opt.Ignore()); // ignorar el id para hacer los update
            CreateMap<kbn_incidencia, incidenciaDTO>();
            CreateMap<kbn_incidencia, idNombreDTO>();
            CreateMap<kbn_incidencia, incidenciaCompletaDTO>();

            CreateMap<nuevaTareaDTO, kbn_tarea>();
            CreateMap<tareaDTO, kbn_tarea>().ForMember(x => x.id, opt => opt.Ignore()); // ignorar el id para hacer los update
            CreateMap<kbn_tarea, tareaDTO>().ForMember(x=>x.nombreTarea,opt=>opt.MapFrom(s=>s.tipoTarea.nombre));

            CreateMap<registroTiempoDTO, kbn_registroTiempo>().ForMember(x => x.id, opt => opt.Ignore()); // ignorar el id para hacer los update
            CreateMap<kbn_registroTiempo, registroTiempoDTO>();

            CreateMap<kbn_tipoIncidencia, idNombreDTO>();

            CreateMap<kbn_tipoTarea, idNombreDTO>();

            CreateMap<kbn_estado, idNombreDTO>();

            CreateMap<registroTiempoDTO, kbn_registroTiempo>();
        }
    }
}
