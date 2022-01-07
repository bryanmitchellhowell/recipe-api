using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using RecipeApp.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace RecipeApi.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public RecipeService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("RecipeAppConnection");
        }

        public bool CreateRecipe(Recipe recipe)
        {
            //using (var conn = new SqlConnection(_configuration.Value))
            using (var conn = new SqlConnection(connectionString))
            {
                const string query = @"insert into tblRecipe (RecipeName) values (@Name)";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    conn.Execute(query, new { recipe.RecipeName }, commandType: CommandType.Text);
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
            return true;
        }
        public bool DeleteRecipe(int id)
        {
            //using (var conn = new SqlConnection(_configuration.Value))
            using (var conn = new SqlConnection(connectionString))
            {
                const string query = @"delete from tblRecipe where RecipeId=@Id";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    conn.Execute(query, new { id }, commandType: CommandType.Text);
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
            return true;
        }
        public bool EditRecipe(int id, Recipe recipe)
        {
            //using (var conn = new SqlConnection(_configuration.Value))
            using (var conn = new SqlConnection(connectionString))
            {
                const string query = @"update tblRecipe set RecipeName = @RecipeName, Description = @RecipeDescription where RecipeId=@Id";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    conn.Execute(query, new { recipe.RecipeName, recipe.RecipeDescription, id }, commandType: CommandType.Text);
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
            return true;
        }
        public IEnumerable<Recipe> GetAllRecipes()
        {
            IEnumerable<Recipe> recipes;
            //using (var conn = new SqlConnection(_configuration.Value))
            using (var conn = new SqlConnection(connectionString))
            {
                const string query = @"select RecipeId, RecipeName, RecipeDescription from tblRecipe";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    recipes = conn.Query<Recipe>(query);

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
            return recipes;
        }
        public IEnumerable<Recipe> GetRecipes(int? RecipeId, string RecipeName)
        {
            IEnumerable<Recipe> recipes;
            //using (var conn = new SqlConnection(_configuration.Value))
            using (var conn = new SqlConnection(connectionString))
            {
                const string cmdQuery = @"upsRecipe";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    recipes = conn.Query<Recipe>(cmdQuery,
                                    new { ii_RecipeId = RecipeId, 
                                        ivc_RecipeName = RecipeName },
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
            return recipes;
        }
        public Recipe SingleRecipe(int id)
        {
            Recipe recipe = new Recipe();

            //using (var conn = new SqlConnection(_configuration.Value))
            using (var conn = new SqlConnection(connectionString))
            {
                const string query = @"select * from tblRecipe where RecipeId =@Id";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    recipe = conn.QueryFirstOrDefault<Recipe>(query, new { id }, commandType: CommandType.Text);
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
            return recipe;
        }                
    }
}

