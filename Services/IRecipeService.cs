using RecipeApp.Shared.Models;
using System.Collections.Generic;

namespace RecipeApi.Services
{
    public interface IRecipeService
    {        
        IEnumerable<Recipe> GetRecipes(int? RecipeId, string RecipeName);
        bool CreateRecipe(Recipe recipe);
        bool EditRecipe(int id, Recipe recipe);        
        Recipe SingleRecipe(int id);
        bool DeleteRecipe(int id);
    }
}
