using System.Collections.Generic;
using RecipeApi.Models;
using RecipeApp.Shared.Models;

namespace RecipeApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);        
        string GetPwd(string pwd);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
