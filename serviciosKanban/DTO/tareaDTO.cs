namespace serviciosKanban.DTO
{
    public class resumenTareaDTO
    {
        public int tipoTareaId { get; set; }
        public string nombreTarea { get; set; }
        public tareaDTO tarea { get; set; }
    }
    public class tareaDTO{
        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public int tipoTareaId { get; set; }
        public string nombreTarea { get; set; }
        public int estadoId { get; set; }
        public int usuarioCreadorId { get; set; }
        public int? usuarioResponsableId { get; set; }
        public int incidenciaId { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int? semanaDeEjecucionPlanificada { get; set; }
        public decimal? estimacion { get; set; }
        public decimal? ejecucion { get; set; }
        public string descripcion { get; set; }
        public int? semanaDeEjecucionReal { get; set; }
        public int proyectoId { get; set; }
    }

    public class tareaCompletaDTO:tareaDTO
    {
        public string nombreUsuarioResponsable { get; set; }
        public string nombreIncidencia { get; set; }
        public string nombreProyecto { get; set; }
        public string nombreEstadoTarea { get; set; }
        public decimal? bloqueo { get; set; }
    }
    public class nuevaTareaDTO
    {
        public string nombre { get; set; } = null!;
        public int tipoTareaId { get; set; }
        public int estadoId { get; set; }
        //public int usuarioCreadorId { get; set; }
        public int? usuarioResponsableId { get; set; }
        public int incidenciaId { get; set; }
        //public DateTime fechaCreacion { get; set; }
        public string descripcion { get; set; }
        public int? semanaDeEjecucionPlanificada { get; set; }
        public decimal? estimacion { get; set; }
        public decimal? ejecucion { get; set; }
        public int? semanaDeEjecucionReal { get; set; }
    }
}