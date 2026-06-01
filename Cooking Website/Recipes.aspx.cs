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

                string skill = _repo.GetUserSkillLevel(userId);
                HiddenDifficulty.Value = skill ?? "";

                var cuisines = _repo.GetUserFavouriteCuisines(userId);
                HiddenCuisines.Value = cuisines.Count > 0
                    ? string.Join(",", cuisines)
                    : "";
            }
        }

        protected string BuildRecipeCardsHtml()
        {
            var sb = new StringBuilder();
            foreach (var r in _repo.GetRecipes())
            {
                sb.Append("<a class=\"recipe-card\"")
                  .Append(" href='/Recipe.aspx?id=").Append(r.Id).Append("'")
                  .Append(" data-difficulty='").Append(Enc(r.Difficulty)).Append("'")
                  .Append(" data-category='").Append(Enc(r.Category)).Append("'")
                  .Append(" data-cuisine='").Append(Enc(r.Cuisine)).Append("'>")
                  .Append("<div class=\"card-image\">").Append(RenderCardImage(r.ImageUrl, r.Title)).Append("</div>")
                  .Append("<div class=\"card-body\">")
                  .Append("<div class=\"card-meta\">")
                  .Append("<span class=\"card-category\">").Append(Enc(r.Category)).Append("</span>")
                  .Append("<span class=\"card-difficulty ").Append(r.Difficulty.ToLower()).Append("\">").Append(Enc(r.Difficulty)).Append("</span>")
                  .Append("</div>")
                  .Append("<h2 class=\"card-title\">").Append(Enc(r.Title)).Append("</h2>")
                  .Append("<p class=\"card-description\">").Append(Enc(r.Description)).Append("</p>")
                  .Append("<div class=\"card-footer\">")
                  .Append("<span class=\"card-time\">&#9201; ").Append(r.PrepTime + r.CookTime).Append(" min</span>")
                  .Append("<span class=\"card-servings\">&#9787; ").Append(r.Servings).Append(" servings</span>")
                  .Append("</div></div></a>");
            }
            return sb.ToString();
        }

        protected string BuildCuisineButtonsHtml()
        {
            var sb = new StringBuilder();
            foreach (var cuisine in _repo.GetDistinctCuisines())
                sb.Append("<button class=\"filter-btn\" data-filter-type=\"cuisine\" data-filter='")
                  .Append(Enc(cuisine)).Append("' type=\"button\">")
                  .Append(Enc(cuisine)).Append("</button>");
            return sb.ToString();
        }

        // HTML-encodes a value for safe inline rendering (mirrors RenderCardImage's alt handling)
        private static string Enc(object value)
        {
            return System.Web.HttpUtility.HtmlEncode(value?.ToString() ?? "");
        }

        // Renders a card image tag, or a placeholder div if no image URL is set
        protected string RenderCardImage(object imageUrl, object title)
        {
            string url = imageUrl?.ToString();
            string alt = System.Web.HttpUtility.HtmlEncode(title?.ToString() ?? "");

            if (!string.IsNullOrEmpty(url))
                return string.Format("<img src='{0}' alt='{1}' loading='lazy' />", url, alt);

            return "<div class='card-image-placeholder'></div>";
        }
    }
}
