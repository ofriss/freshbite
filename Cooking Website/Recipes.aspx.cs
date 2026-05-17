using System;
using System.Web.UI;
using Cooking_Website.DAL;

namespace Cooking_Website
{
    public partial class Recipes : Page
    {
        private readonly RecipeRepository _repo = new RecipeRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLoggedIn = Session["Id"] != null;

            GuestBanner.Visible = !isLoggedIn;

            if (!IsPostBack)
            {
                if (isLoggedIn)
                {
                    int userId = (int)Session["Id"];

                    // Resolve skill level — may be null if not set or unrecognised
                    string skill = _repo.GetUserSkillLevel(userId);
                    HiddenDifficulty.Value = skill ?? "";

                    // Resolve favourite cuisines — may be empty if not set
                    var cuisines = _repo.GetUserFavouriteCuisines(userId);
                    HiddenCuisines.Value = cuisines.Count > 0
                        ? string.Join(",", cuisines)
                        : "";
                }

                BindAll();
            }
        }

        // Renders the card image or a placeholder.
        // Kept in codebehind to avoid escaped quote issues in <%# %> expressions.
        public string RenderCardImage(object imageUrl, object title)
        {
            string url = imageUrl?.ToString();
            string alt = System.Web.HttpUtility.HtmlEncode(title?.ToString() ?? "");

            if (!string.IsNullOrEmpty(url))
                return string.Format("<img src='{0}' alt='{1}' loading='lazy' />", url, alt);

            return "<div class='card-image-placeholder'></div>";
        }

        private void BindAll()
        {
            RecipeRepeater.DataSource = _repo.GetRecipes();
            RecipeRepeater.DataBind();

            CuisineRepeater.DataSource = _repo.GetDistinctCuisines();
            CuisineRepeater.DataBind();
        }
    }
}