namespace serviciosKanban.DTO
{
    public class proyectoDTO{
        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public string codigoProyecto { get; set; } = null!;

    }

    public class nuevoProyectoDTO
    {
        public string nombre { get; set; } = null!;
        public string codigoProyecto { get; set; } = null!;

    }
}