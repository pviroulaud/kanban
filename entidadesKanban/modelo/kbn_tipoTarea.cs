using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_tipoTarea
    {
        public kbn_tipoTarea()
        {
            kbn_tarea = new HashSet<kbn_tarea>();
        }

        public int id { get; set; }
        public string nombre { get; set; } = null!;

        public virtual ICollection<kbn_tarea> kbn_tarea { get; set; }
    }
}
