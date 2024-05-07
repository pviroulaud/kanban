namespace serviciosKanban.DTO
{
    public class usuarioDTO{
        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public string correo { get; set; } = null!;
    }

    public class nuevoUsuarioDTO
    {
        public string nombre { get; set; } = null!;
        public string correo { get; set; } = null!;
    }

    public class loginDTO
    {
        public string usuario { get; set; } = null!;
        public string pass { get; set; } = null!;
    }
    public class respuestaLoginDTO
    {
        public bool autorizado { get; set; }=false;
        public string usuario { get; set; } = "";
        public string tkn { get; set; } = "";
    }
}