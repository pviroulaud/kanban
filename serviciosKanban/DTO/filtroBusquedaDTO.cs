namespace serviciosKanban.DTO
{
    public class filtroBusquedaDTO
    {
        public List<string> incidenciaId { get; set; } = new List<string>();
        public List<string> estadoTareaId { get; set; } = new List<string>();
        public List<string> estadoIncidenciaId { get; set; }=new List<string>();
        public List<string> proyectoId { get; set; }=new List<string>();
        public List<string> tipoIncidenciaId { get; set; }=new List<string>();
        public List<string> tipoTareaId { get; set; }=new List<string>();
        public List<string> usuarioCreadorId { get; set; }=new List<string>();
        public List<string> usuarioResponsableId { get; set; }=new List<string>();
    }
    public class filtroReporte
    {
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public String usuarioId { get; set; } = "";
        public bool verCerrados { get; set; }
    }
    public class filtroReporteSemanal
    {
        public string semana { get; set; }
        public string semanaHasta { get; set; }
        public string usuarioId { get; set; } = "";
        
    }
}