using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shackmeets.Models;

namespace Shackmeets
{
  // Part of Jwt auth tutorial.  Probably not needed, at least in current form.

  public interface IAuthService
  {
    bool UserExists(string username);
    bool AreCredentialsValid(string username, string password);
  }

  public class AuthService : IAuthService
  {
    private readonly ShackmeetsDbContext dbContext;
       
    public AuthService(ShackmeetsDbContext context)
    {
      this.dbContext = context;
    }

    public bool UserExists(string username)
    {
      return this.dbContext.Users.Where(u => u.Username == username).Any();
    }
    
    public bool AreCredentialsValid(string username, string password)
    {
      return true;
    }
  }
}
