using System;
using System.Collections.Generic;

namespace entidadesKanban.modelo
{
    public partial class kbn_usuarioPassword
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public string pass { get; set; } = null!;

        public virtual kbn_usuario usuario { get; set; } = null!;
    }
}
