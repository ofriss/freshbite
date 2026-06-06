using Cooking_Website.DAL;
using System;
using System.Text;
using System.Web.UI;

namespace Cooking_Website
{
    public partial class Recipes : Page
    {
        private readonly RecipeRepository _repo = new RecipeRepository();

        // Sets guest banner visibility and populates hidden profile fields for JS
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLoggedIn = Session["Id"] != null;

            GuestBanner.Visible = !isLoggedIn;

            if (isLoggedIn)
            {
                int userId = (int)Session["Id"];
                HiddenDifficulty.Value = _repo.GetUserSkillLevel(userId) ?? "";
                HiddenCuisines.Value = string.Join(",", _repo.GetUserFavouriteCuisines(userId));
            }
        }

        protected string BuildRecipeCardsHtml()
        {
            var sb = new StringBuilder();
            foreach (var r in _repo.GetRecipes())
            {
                sb.Append($@"
<a class=""recipe-card"" href=""/Recipe.aspx?id={r.Id}""
   data-difficulty=""{r.Difficulty}"" data-category=""{r.Category}"" data-cuisine=""{r.Cuisine}"">
  <div class=""card-image"">{RenderCardImage(r.ImageUrl, r.Title)}</div>
  <div class=""card-body"">
    <div class=""card-meta"">
      <span class=""card-category"">{r.Category}</span>
      <span class=""card-difficulty {r.Difficulty.ToLower()}"">{r.Difficulty}</span>
    </div>
    <h2 class=""card-title"">{r.Title}</h2>
    <p class=""card-description"">{r.Description}</p>
    <div class=""card-footer"">
      <span class=""card-time"">&#9201; {r.PrepTime + r.CookTime} min</span>
      <span class=""card-servings"">&#9787; {r.Servings} servings</span>
    </div>
  </div>
</a>");
            }
            return sb.ToString();
        }

        protected string BuildCuisineButtonsHtml()
        {
            var sb = new StringBuilder();
            foreach (var cuisine in _repo.GetDistinctCuisines())
                sb.Append($@"<button class=""filter-btn"" data-filter-type=""cuisine"" data-filter=""{cuisine}"" type=""button"">{cuisine}</button>");
            return sb.ToString();
        }

        // Renders a card image tag, or a placeholder div if no image URL is set
        protected string RenderCardImage(object imageUrl, object title)
        {
            string url = imageUrl?.ToString();
            if (string.IsNullOrEmpty(url))
                return "<div class='card-image-placeholder'></div>";

            return $"<img src='{url}' alt='{title}' loading='lazy' />";
        }
    }
}
