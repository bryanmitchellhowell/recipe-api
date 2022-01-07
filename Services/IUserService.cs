using System.Collections.Generic;
using RecipeApi.Models;
using RecipeApp.Shared.Models;

namespace RecipeApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);        
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
