namespace serviciosKanban.DTO
{
    public class registroTiempoDTO{
        public int usuarioId { get; set; }
        public int tareaId { get; set; }
        public int estadoTareaId { get; set; }
        public decimal? ejecucion { get; set; }
        public DateTime fechaEjecucion { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string descripcion { get; set; }
    }

}