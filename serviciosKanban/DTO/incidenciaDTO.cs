namespace serviciosKanban.DTO
{
    public class incidenciaDTO{
       public int id { get; set; }
        public string nombre { get; set; } = null!;
        public int tipoIncidenciaId { get; set; }
        public int estadoId { get; set; }
        public int usuarioCreadorId { get; set; }
        public int usuarioResponsableId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int proyectoId { get; set; }
        public string descripcion { get; set; }
    }

    public class incidenciaCompletaDTO:incidenciaDTO
    {
        public List<tareaDTO> tareas{get;set;}

        public List<resumenPorTipoTareaDTO> resumenTareas{get;set;}

        public string nombreProyecto { get; set; }
        public string codigoProyecto { get; set; }

        public string nombreEstado { get; set; }
        public string nombreTipoIncidencia { get; set; }

        public string nombreCreador { get; set; }
        
        public decimal? estimacionTotal { get; set; }
        public decimal? ejecucionTotal { get; set; }
    }
    public class nuevaIncidenciaDTO
    {
        public int proyectoId { get; set; }
        public string nombre { get; set; } = null!;
        public int tipoIncidenciaId { get; set; }
        public int usuarioCreadorId { get; set; }

        public string descripcion { get; set; }

    }
    public class resumenPorTipoTareaDTO
    {
        public int tipoTareaId { get; set; }
        public string nombreTarea { get; set; }
        public int cantidadTareas { get; set; }
        public decimal estimacion { get; set; }
        public decimal ejecucion { get; set; }
    }
}