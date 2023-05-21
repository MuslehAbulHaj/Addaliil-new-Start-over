using Api.Entities.Models;

namespace Api.Entities.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}