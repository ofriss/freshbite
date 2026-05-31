using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cooking_Website.DAL
{
    // Lightweight model for listing recipes (no related data)
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Cuisine { get; set; }
        public string Difficulty { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string ImageUrl { get; set; }  // nullable
    }

    // Full recipe including all related child collections
    public class RecipeDetail : Recipe
    {
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        public List<string> Tips { get; set; }
    }

    public class Ingredient
    {
        public string Quantity { get; set; }
        public string Unit { get; set; }  // nullable
        public string Name { get; set; }
    }

    // DAL for all recipe-related queries; mutations wrap child inserts in a transaction
    public class RecipeRepository
    {
        private readonly string _conn = SqlHelper.LoadConnectionString();

        // ── Helpers ──────────────────────────────────────────────

        // Returns null for DB NULL strings rather than throwing on GetString
        private string SafeString(IDataReader r, string column)
        {
            int ordinal = r.GetOrdinal(column);
            return r.IsDBNull(ordinal) ? null : r.GetString(ordinal);
        }

        // Returns 0 for DB NULL ints rather than throwing on GetInt32
        private int SafeInt(IDataReader r, string column)
        {
            int ordinal = r.GetOrdinal(column);
            return r.IsDBNull(ordinal) ? 0 : r.GetInt32(ordinal);
        }

        // Maps the current reader row to a Recipe object
        private Recipe MapRecipe(IDataReader r)
        {
            return new Recipe
            {
                Id = r.GetInt32(r.GetOrdinal("Id")),
                Title = SafeString(r, "Title"),
                Description = SafeString(r, "Description"),
                Category = SafeString(r, "Category"),
                Cuisine = SafeString(r, "Cuisine"),
                Difficulty = SafeString(r, "Difficulty"),
                PrepTime = SafeInt(r, "PrepTime"),
                CookTime = SafeInt(r, "CookTime"),
                Servings = SafeInt(r, "Servings"),
                ImageUrl = SafeString(r, "ImageUrl")
            };
        }

        // ── Recipes ───────────────────────────────────────────────

        // Returns all recipes ordered newest-first
        public List<Recipe> GetRecipes()
        {
            var list = new List<Recipe>();

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(
                "SELECT * FROM Recipes ORDER BY CreatedAt DESC", con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(MapRecipe(reader));
            }

            return list;
        }

        // Fetches a recipe with all ingredients, steps (ordered), and tips; returns null if not found
        public RecipeDetail GetRecipeById(int id)
        {
            RecipeDetail recipe = null;

            using (var con = new SqlConnection(_conn))
            {
                con.Open();

                // Base recipe
                using (var cmd = new SqlCommand(
                    "SELECT * FROM Recipes WHERE Id = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            recipe = new RecipeDetail
                            {
                                Id = r.GetInt32(r.GetOrdinal("Id")),
                                Title = SafeString(r, "Title"),
                                Description = SafeString(r, "Description"),
                                Category = SafeString(r, "Category"),
                                Cuisine = SafeString(r, "Cuisine"),
                                Difficulty = SafeString(r, "Difficulty"),
                                PrepTime = SafeInt(r, "PrepTime"),
                                CookTime = SafeInt(r, "CookTime"),
                                Servings = SafeInt(r, "Servings"),
                                ImageUrl = SafeString(r, "ImageUrl")
                            };
                        }
                    }
                }

                if (recipe == null) return null;

                // Ingredients
                recipe.Ingredients = new List<Ingredient>();
                using (var cmd = new SqlCommand(
                    "SELECT * FROM RecipeIngredients WHERE RecipeId = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            recipe.Ingredients.Add(new Ingredient
                            {
                                Quantity = SafeString(r, "Quantity"),
                                Unit = SafeString(r, "Unit"),
                                Name = SafeString(r, "Name")
                            });
                }

                // Steps
                recipe.Steps = new List<string>();
                using (var cmd = new SqlCommand(
                    "SELECT Instruction FROM RecipeSteps WHERE RecipeId = @Id ORDER BY StepNumber", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            recipe.Steps.Add(SafeString(r, "Instruction"));
                }

                // Tips
                recipe.Tips = new List<string>();
                using (var cmd = new SqlCommand(
                    "SELECT Tip FROM RecipeTips WHERE RecipeId = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            recipe.Tips.Add(SafeString(r, "Tip"));
                }
            }

            return recipe;
        }

        // ── Filters ───────────────────────────────────────────────

        // Returns distinct cuisines sorted alphabetically with "Other" always last
        public List<string> GetDistinctCuisines()
        {
            var list = new List<string>();

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                SELECT Cuisine FROM (
                    SELECT DISTINCT Cuisine,
                        CASE WHEN Cuisine = 'Other' THEN 1 ELSE 0 END AS SortOrder
                    FROM Recipes
                    WHERE Cuisine IS NOT NULL
                ) AS sub
                ORDER BY SortOrder, Cuisine", con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(SafeString(r, "Cuisine"));
            }

            return list;
        }

        // ── Users ─────────────────────────────────────────────────

        // Returns the user's skill as "Easy", "Medium", or "Hard"; normalises DB variants; null if unknown
        public string GetUserSkillLevel(int userId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(
                "SELECT Skill FROM Users WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", userId);
                con.Open();
                var result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return null;

                switch (result.ToString().Trim().ToLower())
                {
                    case "easy":
                    case "beginner":
                        return "Easy";

                    case "medium":
                    case "intermediate":
                        return "Medium";

                    case "hard":
                    case "advanced":
                    case "expert":
                        return "Hard";

                    default:
                        return null;   // unknown value — don't assume
                }
            }
        }

        // Returns the user's favourite cuisines as a list; parses the comma-delimited Cuisine column
        public List<string> GetUserFavouriteCuisines(int userId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(
                "SELECT Cuisine FROM Users WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", userId);
                con.Open();
                var result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return new List<string>();

                var list = new List<string>();
                foreach (var item in result.ToString().Split(','))
                {
                    var trimmed = item.Trim();
                    if (string.IsNullOrEmpty(trimmed)) continue;

                    // Normalise casing to match the Recipes table
                    var normalised = char.ToUpper(trimmed[0]) + trimmed.Substring(1).ToLower();
                    list.Add(normalised);
                }
                return list;
            }
        }

        // ── Insert ────────────────────────────────────────────────

        // Inserts a recipe and all its child rows in a single transaction; returns the new recipe ID
        public int InsertRecipe(RecipeDetail recipe)
        {
            using (var con = new SqlConnection(_conn))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(@"
                            INSERT INTO Recipes
                                (Title, Description, Category, Cuisine, Difficulty,
                                 PrepTime, CookTime, Servings, ImageUrl)
                            VALUES
                                (@Title, @Description, @Category, @Cuisine, @Difficulty,
                                 @PrepTime, @CookTime, @Servings, @ImageUrl);
                            SELECT SCOPE_IDENTITY();", con, transaction);

                        cmd.Parameters.AddWithValue("@Title", recipe.Title);
                        cmd.Parameters.AddWithValue("@Description", recipe.Description);
                        cmd.Parameters.AddWithValue("@Category", recipe.Category);
                        cmd.Parameters.AddWithValue("@Cuisine", recipe.Cuisine);
                        cmd.Parameters.AddWithValue("@Difficulty", recipe.Difficulty);
                        cmd.Parameters.AddWithValue("@PrepTime", recipe.PrepTime);
                        cmd.Parameters.AddWithValue("@CookTime", recipe.CookTime);
                        cmd.Parameters.AddWithValue("@Servings", recipe.Servings);
                        cmd.Parameters.AddWithValue("@ImageUrl",
                            (object)recipe.ImageUrl ?? DBNull.Value);

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (var ing in recipe.Ingredients)
                        {
                            var ingCmd = new SqlCommand(@"
                                INSERT INTO RecipeIngredients (RecipeId, Quantity, Unit, Name)
                                VALUES (@RecipeId, @Quantity, @Unit, @Name)", con, transaction);

                            ingCmd.Parameters.AddWithValue("@RecipeId", newId);
                            ingCmd.Parameters.AddWithValue("@Quantity", ing.Quantity);
                            ingCmd.Parameters.AddWithValue("@Unit",
                                (object)ing.Unit ?? DBNull.Value);
                            ingCmd.Parameters.AddWithValue("@Name", ing.Name);
                            ingCmd.ExecuteNonQuery();
                        }

                        for (int i = 0; i < recipe.Steps.Count; i++)
                        {
                            var stepCmd = new SqlCommand(@"
                                INSERT INTO RecipeSteps (RecipeId, StepNumber, Instruction)
                                VALUES (@RecipeId, @StepNumber, @Instruction)", con, transaction);

                            stepCmd.Parameters.AddWithValue("@RecipeId", newId);
                            stepCmd.Parameters.AddWithValue("@StepNumber", i + 1);
                            stepCmd.Parameters.AddWithValue("@Instruction", recipe.Steps[i]);
                            stepCmd.ExecuteNonQuery();
                        }

                        if (recipe.Tips != null)
                        {
                            foreach (var tip in recipe.Tips)
                            {
                                var tipCmd = new SqlCommand(@"
                                    INSERT INTO RecipeTips (RecipeId, Tip)
                                    VALUES (@RecipeId, @Tip)", con, transaction);

                                tipCmd.Parameters.AddWithValue("@RecipeId", newId);
                                tipCmd.Parameters.AddWithValue("@Tip", tip);
                                tipCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return newId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // ── Update ────────────────────────────────────────────────

        // Updates recipe header and replaces all child rows (delete + re-insert) in a single transaction
        public void UpdateRecipe(RecipeDetail recipe)
        {
            using (var con = new SqlConnection(_conn))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(@"
                            UPDATE Recipes SET
                                Title       = @Title,
                                Description = @Description,
                                Category    = @Category,
                                Cuisine     = @Cuisine,
                                Difficulty  = @Difficulty,
                                PrepTime    = @PrepTime,
                                CookTime    = @CookTime,
                                Servings    = @Servings,
                                ImageUrl    = @ImageUrl
                            WHERE Id = @Id", con, transaction);

                        cmd.Parameters.AddWithValue("@Id", recipe.Id);
                        cmd.Parameters.AddWithValue("@Title", recipe.Title);
                        cmd.Parameters.AddWithValue("@Description", recipe.Description);
                        cmd.Parameters.AddWithValue("@Category", recipe.Category);
                        cmd.Parameters.AddWithValue("@Cuisine", recipe.Cuisine);
                        cmd.Parameters.AddWithValue("@Difficulty", recipe.Difficulty);
                        cmd.Parameters.AddWithValue("@PrepTime", recipe.PrepTime);
                        cmd.Parameters.AddWithValue("@CookTime", recipe.CookTime);
                        cmd.Parameters.AddWithValue("@Servings", recipe.Servings);
                        cmd.Parameters.AddWithValue("@ImageUrl",
                            (object)recipe.ImageUrl ?? DBNull.Value);
                        cmd.ExecuteNonQuery();

                        // Replace all child rows rather than diffing — simpler and safe inside the transaction
                        foreach (var table in new[] { "RecipeIngredients", "RecipeSteps", "RecipeTips" })
                        {
                            var delCmd = new SqlCommand(
                                $"DELETE FROM {table} WHERE RecipeId = @Id", con, transaction);
                            delCmd.Parameters.AddWithValue("@Id", recipe.Id);
                            delCmd.ExecuteNonQuery();
                        }

                        foreach (var ing in recipe.Ingredients)
                        {
                            var ingCmd = new SqlCommand(@"
                                INSERT INTO RecipeIngredients (RecipeId, Quantity, Unit, Name)
                                VALUES (@RecipeId, @Quantity, @Unit, @Name)", con, transaction);

                            ingCmd.Parameters.AddWithValue("@RecipeId", recipe.Id);
                            ingCmd.Parameters.AddWithValue("@Quantity", ing.Quantity);
                            ingCmd.Parameters.AddWithValue("@Unit",
                                (object)ing.Unit ?? DBNull.Value);
                            ingCmd.Parameters.AddWithValue("@Name", ing.Name);
                            ingCmd.ExecuteNonQuery();
                        }

                        for (int i = 0; i < recipe.Steps.Count; i++)
                        {
                            var stepCmd = new SqlCommand(@"
                                INSERT INTO RecipeSteps (RecipeId, StepNumber, Instruction)
                                VALUES (@RecipeId, @StepNumber, @Instruction)", con, transaction);

                            stepCmd.Parameters.AddWithValue("@RecipeId", recipe.Id);
                            stepCmd.Parameters.AddWithValue("@StepNumber", i + 1);
                            stepCmd.Parameters.AddWithValue("@Instruction", recipe.Steps[i]);
                            stepCmd.ExecuteNonQuery();
                        }

                        if (recipe.Tips != null)
                        {
                            foreach (var tip in recipe.Tips)
                            {
                                var tipCmd = new SqlCommand(@"
                                    INSERT INTO RecipeTips (RecipeId, Tip)
                                    VALUES (@RecipeId, @Tip)", con, transaction);

                                tipCmd.Parameters.AddWithValue("@RecipeId", recipe.Id);
                                tipCmd.Parameters.AddWithValue("@Tip", tip);
                                tipCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}