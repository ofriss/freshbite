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
            if (!int.TryParse(Request.QueryString["id"], out int id))
            {
                Response.Redirect("/Recipes.aspx");
                return;
            }

            CurrentRecipe = _repo.GetRecipeById(id);

            if (CurrentRecipe == null)
            {
                Response.Redirect("/Recipes.aspx");
                return;
            }

            // Hero image
            if (!string.IsNullOrEmpty(CurrentRecipe.ImageUrl))
            {
                HeroImagePanel.Visible = true;
                HeroImage.ImageUrl = CurrentRecipe.ImageUrl;
                HeroImage.AlternateText = CurrentRecipe.Title;
            }
            else
            {
                HeroImagePanel.Visible = false;
            }

            // Tips section visibility
            TipsPanel.Visible = CurrentRecipe.Tips != null && CurrentRecipe.Tips.Count > 0;

            // Resolve user profile for For You badge
            bool isLoggedIn = Session["Id"] != null;
            if (isLoggedIn)
            {
                int userId = (int)Session["Id"];

                string skill = _repo.GetUserSkillLevel(userId);
                var cuisines = _repo.GetUserFavouriteCuisines(userId);

                bool matchesDifficulty = string.IsNullOrEmpty(skill)
                    || GetDifficultyRank(CurrentRecipe.Difficulty) <= GetDifficultyRank(skill);

                bool matchesCuisine = cuisines.Count == 0
                    || cuisines.Contains(CurrentRecipe.Cuisine);

                ForYouPanel.Visible = matchesDifficulty && matchesCuisine;
            }
            else
            {
                ForYouPanel.Visible = false;
            }
        }

        // Renders unit with trailing space, or empty string if null
        protected string RenderUnit(object unit)
        {
            string val = unit?.ToString();
            return !string.IsNullOrEmpty(val) ? val + " " : "";
        }

        protected string BuildIngredientsHtml()
        {
            var sb = new StringBuilder();
            foreach (var ing in CurrentRecipe.Ingredients)
            {
                var qty = Enc(ing.Quantity?.ToString() ?? "");
                var unit = Enc(RenderUnit(ing.Unit));
                sb.Append("<li class=\"ingredient-item\">")
                  .Append("<span class=\"ingredient-check\" aria-hidden=\"true\">&#10003;</span>")
                  .Append("<span class=\"ingredient-text\">")
                  .Append("<span class=\"ingredient-quantity\" data-original='").Append(qty).Append("'>").Append(qty).Append("</span>")
                  .Append(unit)
                  .Append("<span class=\"ingredient-name\">").Append(Enc(ing.Name)).Append("</span>")
                  .Append("</span>")
                  .Append("</li>");
            }
            return sb.ToString();
        }

        protected string BuildStepsHtml()
        {
            var sb = new StringBuilder();
            foreach (var step in CurrentRecipe.Steps)
                sb.Append("<li><div class=\"step-body\">").Append(Enc(step)).Append("</div></li>");
            return sb.ToString();
        }

        protected string BuildTipsHtml()
        {
            var sb = new StringBuilder();
            foreach (var tip in CurrentRecipe.Tips)
                sb.Append("<li><div class=\"tip-body\">").Append(Enc(tip)).Append("</div></li>");
            return sb.ToString();
        }

        // HTML-encodes a value for safe inline rendering (mirrors RenderCardImage's alt handling)
        private static string Enc(object value)
        {
            return System.Web.HttpUtility.HtmlEncode(value?.ToString() ?? "");
        }

        // Maps difficulty to a numeric rank for comparison
        private int GetDifficultyRank(string difficulty)
        {
            if (string.IsNullOrEmpty(difficulty)) return 0;

            switch (difficulty.Trim().ToLower())
            {
                case "easy": return 1;
                case "medium": return 2;
                case "hard": return 3;
                default: return 0;
            }
        }
    }
}
