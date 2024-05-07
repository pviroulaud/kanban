using serviciosKanban.DTO;


namespace serviciosKanban.SRVC
{
    public interface IusuarioSrvc
    {
        List<usuarioDTO> listar();

        int nuevo(int usuarioOperacionId,nuevoUsuarioDTO datos);
        int actualizar(int usuarioOperacionId,usuarioDTO datos);

        bool eliminar(int usuarioOperacionId,int id);

        usuarioDTO? obtener(int id);

        respuestaLoginDTO autenticar(loginDTO credenciales);
    }
    
}