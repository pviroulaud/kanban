using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
namespace serviciosKanban.SRVC
{
    public interface Ijwt
    {
        string GenerarTokenJWT(Dictionary<string, string> ClaimsInfo,
                                            int ID, string correoElectronico,
                                            IConfiguration _configuration,
                                            int horasExpiracion, int minutosExpiracion);


        string GenerarTokenJWT(Claim[] _Claims, int ID, string correoElectronico, IConfiguration _configuration, int horasExpiracion, int minutosExpiracion);


        Dictionary<string, string> getInformacionToken(string JWT_Token);

        Dictionary<string, string> getInformacionToken(System.Security.Principal.IIdentity identidad);

        string getClaimToken(System.Security.Principal.IIdentity identidad, string claimKey);
    }
    
}