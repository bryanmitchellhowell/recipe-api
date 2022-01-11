using Common.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RecipeApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        
        public IEnumerable<Recipe> GetRecipes(int? recipeId, string recipeName)
        {
            IEnumerable<Recipe> recipes = null;
            var param = new
            {
                ii_RecipeId = recipeId,
                ivc_RecipeName = recipeName
            };
            CommonDAL commonDAL = new CommonDAL(connectionString);
            recipes = commonDAL.GetResultListBySP<Recipe>("upsRecipe", param, recipes);
            return recipes;
        }

        public Recipe SingleRecipe(int recipeId)
        {
            Recipe recipe = new Recipe();
                        
            using (var conn = new SqlConnection(connectionString))
            {
                const string query = @"upsRecipeSingle";
                var param = new
                {
                    ii_RecipeId = recipeId
                };

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    using (var mult = conn.QueryMultiple(query, param, commandType: CommandType.StoredProcedure))
                    {
                        recipe = mult.Read<Recipe>().First();
                        var ingredients = mult.Read<RecipeIngredient>();
                        var instructions = mult.Read<RecipeInstruction>();
                        var categories = mult.Read<RecipeCategory>();
                        recipe.RecipeIngredients = ingredients.ToList();
                        recipe.RecipeInstructions = instructions.ToList();
                        recipe.RecipeCategories = categories.ToList();
                    }                    
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

