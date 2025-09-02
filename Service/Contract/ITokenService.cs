using Microsoft.AspNetCore.Identity;

namespace Contract.ITokenService;
public interface ITokenService
{
    string GenerateJwtToken(IdentityUser user, IList<string> roles);
}
