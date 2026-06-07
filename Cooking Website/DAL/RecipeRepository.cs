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
        public int Id { get; set; } // the recipe id
        public string Title { get; set; } // the title of the recipe
        public string Description { get; set; } // the description of the recipe
        public string Category { get; set; } // the category of the recipe (e.g. Meat)
        public string Cuisine { get; set; } // the cuisine of the recipe (e.g. Mexican)
        public string Difficulty { get; set; } // the difficulty of the recipe (e.g. Hard)
        public int PrepTime { get; set; } // the preperation time
        public int CookTime { get; set; } // the cookig time
        public int Servings { get; set; } // the default servings
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

    // DAL for all recipe-related queries. Reads use SqlDataAdapter + DataTable
    public class RecipeRepository
    {
        private readonly string _conn = SqlHelper.LoadConnectionString();

        // ── Helpers ──────────────────────────────────────────────

        // Loads a parameterless query into a DataTable
        private DataTable LoadTable(string sql)
        {
            return LoadTable(sql, null);
        }

        // Loads a query into a DataTable, optionally binding a single @Id parameter
        private DataTable LoadTable(string sql, int? id)
        {
            var table = new DataTable();
            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(sql, con))
            {
                if (id.HasValue)
                    da.SelectCommand.Parameters.AddWithValue("@Id", id.Value);
                da.Fill(table);
            }
            return table;
        }

        // Copies the base recipe columns from a DataRow into a Recipe (or RecipeDetail).
        // Strings use "as string" so a DB NULL becomes null (DBNull is not a string);
        // the nullable int columns fall back to 0 on NULL (note: Convert.ToInt32(DBNull.Value)
        // would throw, hence the explicit guard).
        private void FillRecipe(Recipe r, DataRow row)
        {
            r.Id = Convert.ToInt32(row["Id"]);
            r.Title = row["Title"] as string;
            r.Description = row["Description"] as string;
            r.Category = row["Category"] as string;
            r.Cuisine = row["Cuisine"] as string;
            r.Difficulty = row["Difficulty"] as string;
            r.PrepTime = row["PrepTime"] == DBNull.Value ? 0 : Convert.ToInt32(row["PrepTime"]);
            r.CookTime = row["CookTime"] == DBNull.Value ? 0 : Convert.ToInt32(row["CookTime"]);
            r.Servings = row["Servings"] == DBNull.Value ? 0 : Convert.ToInt32(row["Servings"]);
            r.ImageUrl = row["ImageUrl"] as string;
        }

        // Maps a DataRow to a new Recipe
        private Recipe MapRecipe(DataRow row)
        {
            var r = new Recipe();
            FillRecipe(r, row);
            return r;
        }

        // ── Recipes ───────────────────────────────────────────────

        // Returns all recipes ordered newest-first
        public List<Recipe> GetRecipes()
        {
            var list = new List<Recipe>();

            DataTable table = LoadTable("SELECT * FROM Recipes ORDER BY CreatedAt DESC");
            foreach (DataRow row in table.Rows)
                list.Add(MapRecipe(row));

            return list;
        }

        // Fetches a recipe with all ingredients, steps (ordered), and tips; returns null if not found
        public RecipeDetail GetRecipeById(int id)
        {
            // Base recipe — bail out with null when there's no matching row
            DataTable recipeTable = LoadTable("SELECT * FROM Recipes WHERE Id = @Id", id);
            if (recipeTable.Rows.Count == 0)
                return null;

            var recipe = new RecipeDetail
            {
                Ingredients = new List<Ingredient>(),
                Steps = new List<string>(),
                Tips = new List<string>()
            };
            FillRecipe(recipe, recipeTable.Rows[0]);   // populate the base recipe columns

            // Ingredients
            DataTable ingTable = LoadTable("SELECT * FROM RecipeIngredients WHERE RecipeId = @Id", id);
            foreach (DataRow row in ingTable.Rows)
                recipe.Ingredients.Add(new Ingredient
                {
                    Quantity = row["Quantity"] as string,
                    Unit = row["Unit"] as string,
                    Name = row["Name"] as string
                });

            // Steps (in order)
            DataTable stepTable = LoadTable(
                "SELECT Instruction FROM RecipeSteps WHERE RecipeId = @Id ORDER BY StepNumber", id);
            foreach (DataRow row in stepTable.Rows)
                recipe.Steps.Add(row["Instruction"] as string);

            // Tips
            DataTable tipTable = LoadTable("SELECT Tip FROM RecipeTips WHERE RecipeId = @Id", id);
            foreach (DataRow row in tipTable.Rows)
                recipe.Tips.Add(row["Tip"] as string);

            return recipe;
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

                        // Insert ingredients, steps, and tips for the new recipe
                        InsertChildRows(con, transaction, newId, recipe);

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

                        // Re-insert ingredients, steps, and tips from the updated recipe
                        InsertChildRows(con, transaction, recipe.Id, recipe);

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

        // ── Shared child-row insert ───────────────────────────────

        // Inserts a recipe's ingredients, numbered steps, and tips (tips only when present).
        // Shared by InsertRecipe and UpdateRecipe so the insert logic lives in one place.
        private void InsertChildRows(SqlConnection con, SqlTransaction transaction, int recipeId, RecipeDetail recipe)
        {
            foreach (var ing in recipe.Ingredients)
            {
                var ingCmd = new SqlCommand(@"
                    INSERT INTO RecipeIngredients (RecipeId, Quantity, Unit, Name)
                    VALUES (@RecipeId, @Quantity, @Unit, @Name)", con, transaction);

                ingCmd.Parameters.AddWithValue("@RecipeId", recipeId);
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

                stepCmd.Parameters.AddWithValue("@RecipeId", recipeId);
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

                    tipCmd.Parameters.AddWithValue("@RecipeId", recipeId);
                    tipCmd.Parameters.AddWithValue("@Tip", tip);
                    tipCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
