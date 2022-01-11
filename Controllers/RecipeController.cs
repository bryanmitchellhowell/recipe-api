using Common.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using RecipeApi.Services;
using RecipeApp.Shared.Models;
using System.Collections.Generic;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Success");
        }

        [HttpGet("all")]
        public IActionResult GetAllRecipes()
        {            
            return Ok(_recipeService.GetRecipes(null, ""));
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleRecipe(int id)
        {
            Recipe recipeItem = _recipeService.SingleRecipe(id);
            return Ok(recipeItem);                        
        }

        [HttpGet("recipes/{id?}/{recipeName}")]
        public IActionResult GetRecipes(string id, string recipeName)
        {
            IEnumerable<Recipe> recipeItems = _recipeService.GetRecipes(id.ToIntNull0(), recipeName.APIemptyIfNull());
            return Ok(recipeItems);                        
        }

        [HttpGet("fake")]
        public IActionResult GetFakeRecipe()
        {
            Recipe myRecipe = new Recipe {
                RecipeId = 1,
                RecipeName = "Vegan Quiche",
                RecipeDescription = "Get a recipe for vegan quiche."
            };
            return Ok(myRecipe);
        }


    }
}
