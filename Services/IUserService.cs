using RecipeApi.Models;
using RecipeApp.Shared.Models;
using System.Collections.Generic;

namespace RecipeApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);        
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
