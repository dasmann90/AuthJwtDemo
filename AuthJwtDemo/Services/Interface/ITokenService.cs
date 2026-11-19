using AuthJwtDemo.Models;

namespace AuthJwtDemo.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles); 
    }
}
