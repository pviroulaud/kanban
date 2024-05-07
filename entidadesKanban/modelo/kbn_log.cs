using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_log
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public string entidad { get; set; } = null!;
        public int entidadId { get; set; }
        public string accion { get; set; } = null!;
        public DateTime fechaHora { get; set; }
        public string? detalles { get; set; }
    }
}
