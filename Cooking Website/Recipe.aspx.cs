using Cooking_Website.DAL;
using System;
using System.Web.UI;

namespace Cooking_Website
{
    public partial class Recipe : Page
    {
        public RecipeDetail CurrentRecipe;

        private readonly RecipeRepository _repo = new RecipeRepository();

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

            if (!IsPostBack)
            {
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

                // Bind repeaters
                IngredientsRepeater.DataSource = CurrentRecipe.Ingredients;
                IngredientsRepeater.DataBind();

                StepsRepeater.DataSource = CurrentRecipe.Steps;
                StepsRepeater.DataBind();

                // Only show tips section if recipe has tips
                TipsPanel.Visible = CurrentRecipe.Tips != null && CurrentRecipe.Tips.Count > 0;
                if (TipsPanel.Visible)
                {
                    TipsRepeater.DataSource = CurrentRecipe.Tips;
                    TipsRepeater.DataBind();
                }

                // Resolve user profile for For You badge
                bool isLoggedIn = Session["Id"] != null;
                if (isLoggedIn)
                {
                    int userId = (int)Session["Id"];

                    string skill = _repo.GetUserSkillLevel(userId);
                    HiddenDifficulty.Value = skill ?? "";

                    var cuisines = _repo.GetUserFavouriteCuisines(userId);
                    HiddenCuisines.Value = cuisines.Count > 0
                        ? string.Join(",", cuisines)
                        : "";

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

                // DataBind the page for <%# %> expressions in the header
                Page.DataBind();
            }
        }

        // Renders unit with trailing space, or empty string if null
        protected string RenderUnit(object unit)
        {
            string val = unit?.ToString();
            return !string.IsNullOrEmpty(val) ? val + " " : "";
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