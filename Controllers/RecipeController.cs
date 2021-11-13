using Microsoft.AspNetCore.Mvc;
using RecipeApi.Services;
using RecipeApp.Shared.Models;

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
            return Ok(_recipeService.GetAllRecipes());
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleRecipe(int id)
        {
            Recipe recipeItem = _recipeService.SingleRecipe(id);
            return Ok(recipeItem);

            //return Ok(_recipeService.SingleRecipe(id));
        }

        [HttpGet("fake")]
        public IActionResult GetFakeUser()
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
