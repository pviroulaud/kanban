using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_proyecto
    {
        public kbn_proyecto()
        {
            kbn_incidencia = new HashSet<kbn_incidencia>();
        }

        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public string codigoProyecto { get; set; } = null!;

        public virtual ICollection<kbn_incidencia> kbn_incidencia { get; set; }
    }
}
