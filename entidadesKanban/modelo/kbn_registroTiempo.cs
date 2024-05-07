using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_registroTiempo
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public int tareaId { get; set; }
        public int estadoTareaId { get; set; }
        public decimal? ejecucion { get; set; }
        public string? descripcion { get; set; }
        public DateTime fechaEjecucion { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
