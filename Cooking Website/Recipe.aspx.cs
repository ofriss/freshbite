using Cooking_Website.DAL;
using System;
using System.Text;
using System.Web.UI;

namespace Cooking_Website
{
    public partial class Recipe : Page
    {
        public RecipeDetail CurrentRecipe;

        private readonly RecipeRepository _repo = new RecipeRepository();

        // Resolves recipe by ?id=, binds hero image, and computes For You badge visibility
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if id is passed in URL query
            if (!int.TryParse(Request.QueryString["id"], out int id))
            {
                Response.Redirect("/Recipes.aspx");
                return;
            }

            // Try fetching recipe by id from DB
            CurrentRecipe = _repo.GetRecipeById(id);

            // Check if recipe is not found
            if (CurrentRecipe == null)
            {
                Response.Redirect("/Recipes.aspx");
                return;
            }

            // Hero image
            // If the recipe has an image URL, show the image:
            if (!string.IsNullOrEmpty(CurrentRecipe.ImageUrl))
            {
                HeroImagePanel.Visible = true;
                HeroImage.ImageUrl = CurrentRecipe.ImageUrl;
                HeroImage.AlternateText = CurrentRecipe.Title;
            }
            // Else, hide it
            else
            {
                HeroImagePanel.Visible = false;
            }

            // Tips section visibility
            // If there are any tips, show the panel, else hide it
            TipsPanel.Visible = CurrentRecipe.Tips != null && CurrentRecipe.Tips.Count > 0;
        }

        // Returns the unit followed by a space (e.g. "g "), or just a space when
        // there's no unit — so the quantity is always separated from the name.
        protected string RenderUnit(object unit)
        {
            string val = unit?.ToString();
            return !string.IsNullOrEmpty(val) ? val + " " : " ";
        }

        // Building the HTML of the ingredients list
        protected string BuildIngredientsHtml()
        {
            var sb = new StringBuilder();
            foreach (var ing in CurrentRecipe.Ingredients)
            {
                var qty = ing.Quantity ?? "";
                sb.Append($@"
<li class=""ingredient-item"">
  <span class=""ingredient-check"" aria-hidden=""true"">&#10003;</span>
  <span class=""ingredient-text""><span class=""ingredient-quantity"" data-original='{qty}'>{qty}</span>{RenderUnit(ing.Unit)}<span class=""ingredient-name"">{ing.Name}</span></span>
</li>");
            }
            return sb.ToString();
        }

        // Building the HTML of the steps list
        protected string BuildStepsHtml()
        {
            var sb = new StringBuilder();
            foreach (var step in CurrentRecipe.Steps)
                sb.Append($@"<li><div class=""step-body"">{step}</div></li>");
            return sb.ToString();
        }

        // Building the HTML of the tips list
        protected string BuildTipsHtml()
        {
            var sb = new StringBuilder();
            foreach (var tip in CurrentRecipe.Tips)
                sb.Append($@"<li><div class=""tip-body"">{tip}</div></li>");
            return sb.ToString();
        }
    }
}
