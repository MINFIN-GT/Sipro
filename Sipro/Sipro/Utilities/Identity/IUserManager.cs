using Sipro.Utilities.Identity.Models;
using Microsoft.AspNetCore.Http;

namespace Sipro.Utilities.Identity
{
  public interface IUserManager
  {
    User Validate(string loginTypeCode, string identifier, string secret);
    void SignIn(HttpContext httpContext, User user, bool isPersistent = false);
    void SignOut(HttpContext httpContext);
    int GetCurrentUserId(HttpContext httpContext);
    User GetCurrentUser(HttpContext httpContext);
  }
}