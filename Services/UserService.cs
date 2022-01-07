using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using RecipeApi.Helpers;
using RecipeApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using RecipeApp.Shared.Models;
using Common.Shared.Utilities;

namespace RecipeApi.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
        private readonly string secret;

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test" }
        };
                
        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("RecipeAppConnection");
            secret = _configuration.GetSection("AppSettings")["Secret"];
        }
        
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            User user;
            Encrypter encrypter = new Encrypter();
            string pwd = encrypter.EncryptWord(model.Password);
            

            //Look up user in db, must also see if user exists, but pwd does not match
            using (var conn = new SqlConnection(connectionString))
            {
                const string cmdQuery = @"upsLogin";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    user = conn.QueryFirstOrDefault<User>(cmdQuery,
                                    new
                                    {
                                        ivc_UserName = model.Username,
                                        ivc_Password = pwd
                                    },
                                    commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            
            // return null if user not found
            if (user == null) return null;
            if (!user.ErrorMessage.IsNullOrEmpty()) return new AuthenticateResponse(user, null);

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {

            return _users.FirstOrDefault(x => x.Id == id);
        }
        
        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();            
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}