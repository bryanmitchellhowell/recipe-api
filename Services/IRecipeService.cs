using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApp.Shared.Models;

namespace RecipeApi.Services
{
    public interface IRecipeService
    {
        IEnumerable<Recipe> GetAllRecipes();
        IEnumerable<Recipe> GetRecipes(int? RecipeId, string RecipeName);
        bool CreateRecipe(Recipe recipe);
        bool EditRecipe(int id, Recipe recipe);        
        Recipe SingleRecipe(int id);
        bool DeleteRecipe(int id);
    }
}
