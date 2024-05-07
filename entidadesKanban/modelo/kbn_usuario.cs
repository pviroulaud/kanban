using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_usuario
    {
        public kbn_usuario()
        {
            kbn_incidenciausuarioCreador = new HashSet<kbn_incidencia>();
            kbn_incidenciausuarioResponsable = new HashSet<kbn_incidencia>();
            kbn_tareausuarioCreador = new HashSet<kbn_tarea>();
            kbn_tareausuarioResponsable = new HashSet<kbn_tarea>();
            kbn_usuarioPassword = new HashSet<kbn_usuarioPassword>();
        }

        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public string correo { get; set; } = null!;

        public virtual ICollection<kbn_incidencia> kbn_incidenciausuarioCreador { get; set; }
        public virtual ICollection<kbn_incidencia> kbn_incidenciausuarioResponsable { get; set; }
        public virtual ICollection<kbn_tarea> kbn_tareausuarioCreador { get; set; }
        public virtual ICollection<kbn_tarea> kbn_tareausuarioResponsable { get; set; }
        public virtual ICollection<kbn_usuarioPassword> kbn_usuarioPassword { get; set; }
    }
}
