# FreshBite — Cooking Website
## Project Documentation

**Date:** 2026-05-30  
**Author:** Documenter Agent  
**Platform:** ASP.NET Web Forms, C#, SQL Server

---

## Table of Contents

1. [Section 1 — Class Tables](#section-1--class-tables)
   - [.NET / Framework Classes](#net--framework-classes)
     - [SqlConnection](#sqlconnection)
     - [SqlCommand](#sqlcommand)
     - [SqlDataAdapter](#sqldataadapter)
     - [SqlDataReader](#sqldatareader)
     - [SqlTransaction](#sqltransaction)
     - [DataSet](#dataset)
     - [DataRow](#datarow)
     - [DataColumn](#datacolumn)
     - [Dictionary\<K,V\>](#dictionarykv)
     - [List\<T\>](#listt)
     - [DateTime](#datetime)
     - [Regex](#regex)
     - [JavaScriptSerializer](#javascriptserializer)
     - [ConfigurationManager](#configurationmanager)
     - [HttpSessionState (Session)](#httpsessionstate-session)
     - [HttpApplicationState (Application)](#httpapplicationstate-application)
     - [HttpResponse (Response)](#httpresponse-response)
     - [HttpRequest (Request)](#httprequest-request)
     - [File (System.IO)](#file-systemio)
     - [Math](#math)
     - [Convert](#convert)
     - [Guid](#guid)
   - [Custom Project Classes](#custom-project-classes)
     - [Recipe](#recipe)
     - [RecipeDetail](#recipedetail)
     - [Ingredient](#ingredient)
     - [RecipeRepository](#reciperepository)
     - [UsersRepository](#usersrepository)
     - [SqlHelper](#sqlhelper)
     - [QuizQuestion](#quizquestion)
     - [QuizHelper](#quizhelper)
2. [Section 2 — Page Descriptions](#section-2--page-descriptions)
   - [Index.aspx](#indexaspx)
   - [Login.aspx](#loginaspx)
   - [Register.aspx](#registeraspx)
   - [Profile.aspx](#profileaspx)
   - [Recipes.aspx](#recipesaspx)
   - [Recipe.aspx](#recipeaspx)
   - [Gallery.aspx](#galleryaspx)
   - [Tips.aspx](#tipsaspx)
   - [Quiz.aspx](#quizaspx)
   - [QuizResults.aspx](#quizresultsaspx)
   - [Admin/ViewUsers.aspx](#adminviewusersaspx)
   - [Admin/ManageUsers.aspx](#adminmanageusersaspx)
   - [Admin/Statistics.aspx](#adminstatisticsaspx)
   - [Guides/MeatGuide.aspx](#guidesmeatguideaspx)
   - [Guides/PoultryGuide.aspx](#guidespoultryguideaspx)
   - [Guides/FishGuide.aspx](#guidesfishguideaspx)
   - [Guides/VegetablesGuide.aspx](#guidesvegetablesguideaspx)
   - [Guides/EggsGuide.aspx](#guideseggsguidespx)
   - [Guides/PastaGuide.aspx](#guidespastaaspx)
   - [Guides/BakingGuide.aspx](#guidesbakingguideaspx)
   - [Guides/SoupsGuide.aspx](#guidessoupsguideaspx)
   - [Guides/SaucesGuide.aspx](#guidessaucesguideaspx)
   - [Utilities/Timer.aspx](#utilitiestimeraspx)
   - [Utilities/UnitsCalculator.aspx](#utilitiesunitscalculatoraspx)

---

---

# Section 1 — Class Tables

---

## .NET / Framework Classes

---

### SqlConnection
`System.Data.SqlClient`

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| (constructor arg) connection string | Passed the connection string returned by `SqlHelper.LoadConnectionString()` to identify the target database. |

#### Methods

| Method | Documentation |
|---|---|
| `Open()` | Opens the physical connection to the SQL Server database before any commands are executed. |
| `BeginTransaction()` | Starts a database transaction in `RecipeRepository`; used when inserting or updating a recipe and its related rows so all changes succeed or all are rolled back together. |
| `CreateCommand()` | Factory method used in `UsersRepository` and `Profile.aspx.cs` to create a `SqlCommand` already associated with the connection. |

---

### SqlCommand
`System.Data.SqlClient`

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `CommandText` | Set to the SQL query string when `CreateCommand()` is used instead of the constructor. |
| `Parameters` | Collection of `SqlParameter` objects. All queries in this project use parameterised values via `AddWithValue` to prevent SQL injection. |

#### Methods

| Method | Documentation |
|---|---|
| `Parameters.AddWithValue(name, value)` | Adds a named parameter and its value to the command. Used throughout the project for every dynamic query value (usernames, passwords, recipe fields, etc.). |
| `ExecuteReader()` | Executes a SELECT query and returns a `SqlDataReader` for row-by-row reading. Used in `RecipeRepository` and `UsersRepository`. |
| `ExecuteScalar()` | Executes a query and returns the single first-column, first-row value. Used to get a user ID after INSERT, check whether a user exists, or fetch a single user field. |
| `ExecuteNonQuery()` | Executes INSERT, UPDATE, or DELETE statements where no result set is needed. Used for all write operations (create/update/delete user, delete recipe child rows, etc.). |

---

### SqlDataAdapter
`System.Data.SqlClient`

#### Methods

| Method | Documentation |
|---|---|
| `Fill(DataSet)` | Executes the adapter's SELECT query and populates a `DataSet` with the result. Used in `ViewUsers.aspx.cs` and `ManageUsers.aspx.cs` to load the full users table into memory for HTML generation. |

---

### SqlDataReader
`System.Data.SqlClient`

#### Methods

| Method | Documentation |
|---|---|
| `Read()` | Advances the reader to the next row. Called in `while` loops in `RecipeRepository` to iterate all result rows. |
| `GetInt32(ordinal)` | Reads an integer column by its positional index. Used to read the `Id` column in recipe queries. |
| `GetString(ordinal)` | Reads a string column by its positional index. Wrapped by `SafeString()` in `RecipeRepository` to handle nulls. |
| `GetOrdinal(columnName)` | Looks up a column's positional index by name. Used in `RecipeRepository`'s helper methods so columns are accessed by name but read by position. |
| `IsDBNull(ordinal)` | Returns true if the value at the given column position is a database NULL. Used in `SafeString` and `SafeInt` to return `null` or `0` instead of throwing an exception. |
| `indexer [columnName]` | Reads a column by name and returns it as `object`. Used in `UsersRepository.GetRow()` for flexible dictionary building. |

---

### SqlTransaction
`System.Data.SqlClient`

#### Methods

| Method | Documentation |
|---|---|
| `Commit()` | Permanently saves all changes made since `BeginTransaction()`. Called in `InsertRecipe` and `UpdateRecipe` once all related rows have been written successfully. |
| `Rollback()` | Discards all changes since `BeginTransaction()`. Called in the `catch` blocks of `InsertRecipe` and `UpdateRecipe` if any part of the multi-table write fails. |

---

### DataSet
`System.Data`

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `Tables[0]` | Accesses the first (and only) table in the dataset. Used to iterate columns and rows when building the users HTML table in `ViewUsers` and `ManageUsers`. |

---

### DataRow
`System.Data`

#### Methods

| Method | Documentation |
|---|---|
| `indexer [columnName]` | Reads a field value by column name. Used in `ManageUsers.aspx.cs` to extract each user's fields when building the table HTML. |

---

### DataColumn
`System.Data`

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `ColumnName` | The name of the column. Used in `ViewUsers.aspx.cs` to build table header cells dynamically from the result set, with special handling to skip the `Password` column. |

---

### Dictionary\<K,V\>
`System.Collections.Generic`

Used in several places with different type parameters.

#### Methods

| Method | Documentation |
|---|---|
| `Add(key, value)` | Adds a key-value pair. Used in `UsersRepository.GetRow()` to build the user profile dictionary, and in `Profile.aspx.cs` to build the form-data and changed-fields dictionaries. |
| `ContainsKey(key)` | Checks whether a key exists. Used in `CompareToProfile()` in `Profile.aspx.cs` to safely look up whether the profile already has a given field before comparing values. |
| `Where(predicate).ToDictionary(...)` | Filters the dictionary to entries whose values differ from the stored profile, returning only the fields that actually changed. Used in `CompareToProfile()`. |
| `Count` | Number of entries. Checked in `Profile.aspx.cs` to skip the database update when nothing has changed. |
| `Keys` | Enumerable of all keys. Used in `Profile.aspx.cs` to build the dynamic `SET` clause of an UPDATE statement. |
| `indexer [key]` | Retrieves a value by key. Used when reading quiz results (`Session["QuizResults"]`) and when iterating field values in the profile update. |

---

### List\<T\>
`System.Collections.Generic`

#### Methods

| Method | Documentation |
|---|---|
| `Add(item)` | Appends an item. Used throughout `RecipeRepository` to accumulate recipe rows, ingredients, steps, tips, cuisines, and quiz questions. |
| `Count` | Number of items. Used to decide whether to show the Tips section on `Recipe.aspx`, to drive the quiz question loop, and to check whether cuisine preferences are set. |
| `Contains(item)` | Checks for membership. Used in `Recipe.aspx.cs` to test whether the recipe's cuisine is in the user's favourite cuisines list. |
| `IndexOf(item)` | Finds the position of an item. Used in `Quiz.aspx.cs` to locate the correct answer after the answer list has been shuffled. |
| `OrderBy(selector).ToList()` | Sorts the list and returns a new list. Used in `Quiz.aspx.cs` to randomise question and answer order. |
| `Select(transform).ToList()` | Projects each item through a function and returns a new list. Used in `QuizHelper` to produce a deep copy of the question list and in `Quiz.aspx.cs` to shuffle answers per question. |

---

### DateTime
`System`

#### Methods / Members

| Method | Documentation |
|---|---|
| `DateTime.Now` | Gets the current date and time. Used in `Quiz.aspx.cs` to record the start time and to calculate elapsed time when the quiz is submitted. |
| `DateTime.TryParse(s, out result)` | Tries to parse a date string. Used in `Register.aspx.cs` and `ManageUsers.aspx.cs` when converting birthday input from a form field. |
| `DateTime.Parse(s)` | Parses a date string, throwing on failure. Used in `ManageUsers.aspx.cs` when it is known the date string was already validated. |
| `DateTime.MinValue` | The earliest representable `DateTime`. Used as a fallback in `Register.aspx.cs` when a birthday cannot be parsed, to avoid storing a null. |
| `ToString(format)` | Formats the date as a string. Used in `ManageUsers.aspx.cs` to render birthdays as `dd MMM yyyy` (display) and `yyyy-MM-dd` (form value). |
| Subtraction (`DateTime - DateTime`) | Computes a `TimeSpan`. Used in `Quiz.aspx.cs` to calculate how long the user took to complete the quiz. |

---

### Regex
`System.Text.RegularExpressions`

#### Methods

| Method | Documentation |
|---|---|
| `Regex.IsMatch(input, pattern)` | Tests whether the input matches the pattern. Used in both `Login.aspx.cs` and `Register.aspx.cs` to check that usernames contain only alphanumeric characters, and that passwords do not contain characters outside the allowed set (`a-z A-Z 0-9 ! * @ _ $ #`). |

---

### JavaScriptSerializer
`System.Web.Script.Serialization`

#### Methods

| Method | Documentation |
|---|---|
| `Deserialize<T>(json)` | Parses a JSON string into a strongly-typed object. Used in `QuizHelper.LoadQuizQuestions()` to deserialise `quiz_questions.json` into a `List<QuizQuestion>`. |

---

### ConfigurationManager
`System.Configuration`

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `ConnectionStrings["DefaultConnection"]` | Reads the named connection string entry from `Web.config`. Used exclusively by `SqlHelper.LoadConnectionString()`. |

---

### HttpSessionState (Session)
`System.Web` — accessed via the `Session` property on any `Page`

#### Usage in this project

| Key | What is stored |
|---|---|
| `Session["Id"]` | The logged-in user's database ID (`int`). Presence of this key indicates the user is authenticated. |
| `Session["Username"]` | The logged-in user's username string. Displayed in the navigation bar and on the Quiz page. |
| `Session["Questions"]` | The shuffled `List<QuizQuestion>` for the active quiz, persisted across postbacks. |
| `Session["QuizStart"]` | A `DateTime` recording when the Start button was clicked, used to calculate elapsed time. |
| `Session["QuizResults"]` | A `Dictionary<int, bool>` mapping question number to correct/wrong result, written by `Quiz.aspx.cs` and read by `QuizResults.aspx.cs`. |
| `Session["QuizTimer"]` | Formatted time string (`mm:ss`) of quiz duration, written after submit and displayed on results page. |

#### Methods

| Method | Documentation |
|---|---|
| `Session.Clear()` | Removes all values from the session. Called during logout to wipe user data. |
| `Session.Abandon()` | Ends the session entirely and triggers `Session_End` in `Global.asax.cs`. Called after `Session.Clear()` during logout. |

---

### HttpApplicationState (Application)
`System.Web` — accessed via the `Application` property

#### Usage in this project

| Key | What is stored |
|---|---|
| `Application["Online"]` | Integer count of currently active sessions (users with any open session, logged in or not). Incremented on `Session_Start`, decremented on `Session_End`. |
| `Application["LoggedIn"]` | Integer count of sessions that have a user ID — i.e., authenticated users. Incremented on login/register, decremented on logout or session end. |

---

### HttpResponse (Response)
`System.Web` — accessed via the `Response` property on any `Page`

#### Methods

| Method | Documentation |
|---|---|
| `Response.Redirect(url)` | Sends a 302 redirect to the browser. Used project-wide for navigation after login, registration, logout, and when invalid recipe IDs are requested. |
| `Response.Write(text)` | Writes text directly into the response stream. Used on `Index.aspx` to inline the username greeting inside the hero heading. |

---

### HttpRequest (Request)
`System.Web` — accessed via the `Request` property on any `Page`

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `Request.Form[name]` | Reads a raw POST form field by name. Used in `Login.aspx.cs`, `Register.aspx.cs`, `Profile.aspx.cs`, and `ManageUsers.aspx.cs` to read submitted form values. |
| `Request.QueryString["id"]` | Reads a URL query-string parameter. Used in `Recipe.aspx.cs` to get the recipe ID from the URL (e.g. `Recipe.aspx?id=5`). |

---

### File (System.IO)

#### Methods

| Method | Documentation |
|---|---|
| `File.ReadAllText(path)` | Reads the entire contents of a file as a string. Used in `QuizHelper.LoadQuizQuestions()` to load the `quiz_questions.json` data file from the server's file system. |

---

### Math
`System`

#### Methods

| Method | Documentation |
|---|---|
| `Math.PI` | The mathematical constant π. Used in `QuizResults.aspx.cs` to calculate the circumference of the SVG progress ring circle. |

---

### Convert
`System`

#### Methods

| Method | Documentation |
|---|---|
| `Convert.ToInt32(value)` | Converts a database scalar result to an integer. Used in `RecipeRepository.InsertRecipe()` to get the new record's ID after INSERT, and in `ManageUsers.aspx.cs` to parse row IDs. |
| `Convert.ToDateTime(value)` | Converts a database object to a `DateTime`. Used in `ManageUsers.aspx.cs` when reading birthday values from a `DataRow`. |

---

### Guid
`System`

#### Methods

| Method | Documentation |
|---|---|
| `Guid.NewGuid()` | Generates a new random GUID. Used in `Quiz.aspx.cs` as the sort key for `OrderBy` to produce a random shuffle of both the question list and each question's answer list. |

---

---

## Custom Project Classes

---

### Recipe
`Cooking_Website.DAL` — file: `DAL/RecipeRepository.cs`

The base model class representing a single recipe row from the `Recipes` database table. All properties map directly to database columns.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `Id` | The recipe's primary key in the database. |
| `Title` | The display name of the recipe. |
| `Description` | A short summary paragraph shown on the recipe card and detail page. |
| `Category` | Broad type of dish (e.g., "Dinner", "Dessert"). Shown as a tag on the detail page. |
| `Cuisine` | The culinary tradition the recipe belongs to (e.g., "Italian", "Japanese"). Used for the cuisine filter on the Recipes page and for "For You" matching. |
| `Difficulty` | Skill level required: "Easy", "Medium", or "Hard". Used for filtering and "For You" matching. |
| `PrepTime` | Preparation time in minutes. |
| `CookTime` | Cooking time in minutes. |
| `Servings` | Default number of servings; used as the starting value for the servings scaler on `Recipe.aspx`. |
| `ImageUrl` | Optional URL to the recipe's hero image. Null if the recipe has no image. |

---

### RecipeDetail
`Cooking_Website.DAL` — file: `DAL/RecipeRepository.cs`

Extends `Recipe` with the full detail data needed to render `Recipe.aspx`. The extra lists are populated by three additional database queries when `GetRecipeById` is called.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `Ingredients` | List of `Ingredient` objects for the recipe. |
| `Steps` | Ordered list of instruction strings, each representing one step of the method. |
| `Tips` | Optional list of tip strings. If empty or null the Tips section is hidden on the page. |

---

### Ingredient
`Cooking_Website.DAL` — file: `DAL/RecipeRepository.cs`

Represents one ingredient row from the `RecipeIngredients` table.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `Quantity` | The amount needed (e.g., "2", "1/2"). Rendered with a `data-original` attribute so the servings scaler JavaScript can scale it proportionally. |
| `Unit` | Optional unit of measurement (e.g., "cup", "tsp"). Null if no unit applies. |
| `Name` | The ingredient name (e.g., "flour", "olive oil"). |

---

### RecipeRepository
`Cooking_Website.DAL` — file: `DAL/RecipeRepository.cs`

The data access class for all recipe and user-preference queries. Instantiated as a private field in `Recipes.aspx.cs` and `Recipe.aspx.cs`.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `_conn` (private) | The connection string, loaded once from `SqlHelper` at construction time. |

#### Methods

| Method | Documentation |
|---|---|
| `SafeString(reader, column)` | Private helper. Reads a string column by name, returning `null` if the database value is NULL instead of throwing an exception. |
| `SafeInt(reader, column)` | Private helper. Reads an integer column by name, returning `0` if the database value is NULL. |
| `MapRecipe(reader)` | Private helper. Maps the current `SqlDataReader` row to a new `Recipe` object using `SafeString` and `SafeInt`. |
| `GetRecipes()` | Returns all recipes from the database ordered by most recently created. Used to populate the recipe card grid on `Recipes.aspx`. |
| `GetRecipeById(id)` | Returns the full detail for one recipe including ingredients, steps, and tips. Returns `null` if the ID does not exist. Used by `Recipe.aspx.cs`. |
| `GetDistinctCuisines()` | Returns a sorted list of all distinct cuisine values, with "Other" forced to the end. Used to build the cuisine filter buttons on `Recipes.aspx`. |
| `GetUserSkillLevel(userId)` | Reads the `Skill` column for a user and normalises it to "Easy", "Medium", or "Hard". Returns `null` for unrecognised values. Used for the "For You" badge logic. |
| `GetUserFavouriteCuisines(userId)` | Reads the comma-separated `Cuisine` column for a user and returns it as a `List<string>` with normalised title casing. Used for the "For You" badge logic. |
| `InsertRecipe(recipe)` | Inserts a full `RecipeDetail` (recipe row + ingredients + steps + tips) inside a single database transaction. Returns the new recipe's ID. |
| `UpdateRecipe(recipe)` | Updates a recipe's main row, then deletes and re-inserts its child rows (ingredients, steps, tips) inside a single transaction — the simplest correct approach for replacing lists. |

---

### UsersRepository
`Cooking_Website.DAL` — file: `DAL/UsersRepository.cs`

Static utility class for user authentication and data retrieval. All methods are `static` — no instance needed.

#### Methods

| Method | Documentation |
|---|---|
| `DoesUserExist(username)` | Returns `true` if a row with the given username already exists in the `Users` table. Used in `Register.aspx.cs` before inserting to prevent duplicate usernames. |
| `GetRow(id)` | Fetches a single user row by ID and returns it as a `Dictionary<string, object>`. The `Cuisine` field is split into a `string[]` on return. Used by `Profile.aspx.cs` to load the current user's data. |
| `CheckCredentials(username, password)` | Overload that checks credentials without returning the ID. Used in `Profile.aspx.cs` to verify the current password before allowing a password change. |
| `CheckCredentials(username, password, out id)` | Queries the `Users` table for a matching username and password. Sets `id` to the user's database ID on success; sets it to `0` and returns `false` on failure. Used in `Login.aspx.cs`. |

---

### SqlHelper
`Cooking_Website.Helpers` — file: `Helpers/SqlHelper.cs`

A small static helper that caches the database connection string so it is only read from `Web.config` once per application lifetime.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `isLoaded` (private static) | Flag that prevents repeated `Web.config` reads. Set to `true` after the first successful load. |
| `connStr` (private static) | The cached connection string value. |

#### Methods

| Method | Documentation |
|---|---|
| `LoadConnectionString()` | Returns the `DefaultConnection` connection string from `Web.config`. Throws `InvalidOperationException` if the key is missing. Caches the result so subsequent calls skip the configuration read. |

---

### QuizQuestion
`Cooking_Website.Helpers` — file: `Helpers/QuizHelper.cs`

Model class representing a single quiz question as loaded from `Data/quiz_questions.json`.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `Id` | Unique question identifier from the JSON file. |
| `Question` | The question text displayed to the user. |
| `Answers` | List of four answer strings. The list is shuffled per-quiz in `Quiz.aspx.cs`. |
| `Correct` | Index into `Answers` pointing to the correct answer. Updated after shuffling so it still points to the right answer. |

---

### QuizHelper
`Cooking_Website.Helpers` — file: `Helpers/QuizHelper.cs`

Static helper that loads and caches quiz questions from a JSON file.

#### Properties / Fields

| Property/Field | Documentation |
|---|---|
| `questions` (private static) | Cached parsed question list. Null on first call; populated by the first call to `LoadQuizQuestions()`. |

#### Methods

| Method | Documentation |
|---|---|
| `LoadQuizQuestions()` | Reads `Data/quiz_questions.json` from disk and deserialises it using `JavaScriptSerializer`. After the first call the raw list is cached; every call returns a fresh **deep copy** of the cached list so that shuffling in `Quiz.aspx.cs` never corrupts the original. |

---

---

# Section 2 — Page Descriptions

---

## Index.aspx

**Code-behind:** `Index.aspx.cs` | **Master page:** `Site1.Master`

The home page of the FreshBite website. It displays a large hero section with the site name, a short tagline, and an "Explore Recipes" call-to-action button. Below the hero, three feature cards introduce the site's main areas: Cooking Guides, Recipes, and Kitchen Tools (Calculator and Timer). If the user is not logged in, an additional call-to-action strip is shown at the bottom inviting them to create an account or sign in, explaining that registered users receive personalised recipe suggestions matched to their skill level and preferred cuisines. When the user is logged in their username is inserted into the welcome heading by a server-side `Response.Write` expression. The code-behind for this page is empty — all dynamic behaviour is handled directly in the markup with inline `<% %>` expressions.

---

## Login.aspx

**Code-behind:** `Login.aspx.cs` | **Master page:** `Site1.Master`

The sign-in page. It presents a form with a username field and a password field. Both fields perform live client-side validation via `login.js` as the user types, and there is a final check on submit. On the server side, `Page_Load` runs validation again when a POST is detected. Validation checks that the username is at least four characters, alphanumeric only, and has no spaces; and that the password is at least eight characters, contains both uppercase and lowercase letters, allows only the special characters `! * @ _ $ #`, and has no spaces. If validation passes, `UsersRepository.CheckCredentials` is called; on success, `Session["Id"]` and `Session["Username"]` are set, `Application["LoggedIn"]` is incremented, and the user is redirected to the home page. If credentials do not match or validation fails, a red error message is displayed via an `asp:Label`. This page is accessible to anyone (no login guard).

---

## Register.aspx

**Code-behind:** `Register.aspx.cs` | **Master page:** `Site1.Master`

The new-account registration page. The form collects: username, password, date of birth, gender (radio buttons: Male/Female), favourite cuisine(s) (checkboxes for Italian, Indian, Japanese, Mediterranean, Mexican, American, Chinese, French, Other), cooking skill level (Beginner, Intermediate, Advanced), and agreement to terms of use. Client-side validation via `register.js` provides immediate feedback as the user fills in the form. Server-side validation in `ValidateForm()` applies identical rules to the login page for username and password, and additionally checks that birthday, gender, skill level, and terms are present and valid. If all checks pass, `InsertUserIntoDatabase()` first calls `UsersRepository.DoesUserExist` to reject duplicate usernames; if the username is free it inserts the new user row with `OUTPUT INSERTED.Id` to retrieve the new ID in one round trip. On success the user is immediately logged in (session is set, `Application["LoggedIn"]` incremented) and redirected to the home page. This page is accessible to anyone.

---

## Profile.aspx

**Code-behind:** `Profile.aspx.cs` | **Master page:** `Site1.Master`

The account profile page, accessible only to logged-in users. `Page_Load` redirects unauthenticated visitors to `Login.aspx`. On every load it calls `UsersRepository.GetRow()` to populate the `profileInfo` dictionary with the current user's data (username, birthday, gender, cuisine preferences, skill level), which the markup uses to pre-fill the form fields. When the Save button is submitted, `saveBtn_Click` reads the new form values, calls `CompareToProfile()` to identify which fields actually changed, and optionally handles a password change (requires the current password to be correct, verified via `UsersRepository.CheckCredentials`). If any fields changed, `Update()` builds a dynamic SQL `UPDATE` statement targeting only those fields, executes it, and then refreshes `profileInfo` from the database. The session username is also updated if the username was changed. A server-side message label reports success or failure. The cuisine preference is stored in the database as a comma-separated string and split back into an array on read.

---

## Recipes.aspx

**Code-behind:** `Recipes.aspx.cs` | **Master page:** `Site1.Master`

The main recipe browsing page. On first load, `Page_Load` checks whether the user is logged in. If logged in, it retrieves the user's skill level and favourite cuisines via `RecipeRepository` and writes them into hidden fields (`HiddenDifficulty` and `HiddenCuisines`); the client-side JavaScript on the page reads these to highlight matching cards with a "For You" badge. A guest banner panel is shown when the user is not logged in, encouraging registration. The `BindAll()` method populates two repeaters: `RecipeRepeater` (a card grid of all recipes ordered by newest first) and `CuisineRepeater` (filter buttons for each distinct cuisine in the database, with "Other" sorted last). A `RenderCardImage()` helper method in the code-behind generates the card image tag or a placeholder div, depending on whether the recipe has an `ImageUrl`. Accessible to everyone.

---

## Recipe.aspx

**Code-behind:** `Recipe.aspx.cs` | **Master page:** `Site1.Master`

The full recipe detail page. It is reached with a query string parameter: `Recipe.aspx?id=N`. If the `id` parameter is missing or not a valid integer, or if no recipe with that ID exists in the database, the page immediately redirects back to `Recipes.aspx`. On a valid request, `GetRecipeById()` is called to load the full `RecipeDetail` object. The page renders: an optional hero image, tag badges for category/cuisine/difficulty, the recipe title and description, a stats row showing prep time, cook time, total time, and a servings scaler (the scaler is JavaScript-powered and proportionally adjusts ingredient quantities). Below the stats, three sections are rendered using `asp:Repeater` controls: Ingredients (with click-to-check-off behaviour), Method steps (ordered list), and Tips (shown only if the recipe has tips, controlled by `TipsPanel.Visible`). For logged-in users the code checks whether the recipe matches their skill level and cuisine preferences; if it does, a "For You" badge is shown in the tag row. The difficulty comparison uses numeric ranks (Easy=1, Medium=2, Hard=3) so that a user rated "Medium" sees badges on Easy and Medium recipes but not Hard ones. Accessible to everyone, but the "For You" badge only appears for logged-in users.

---

## Gallery.aspx

**Code-behind:** `Gallery.aspx.cs` | **Master page:** `Site1.Master`

A food photo gallery page. The markup contains 15 food images sourced from Unsplash, rendered inside a single `div.frame`. Navigation is handled entirely by `gallery.js`: Previous and Next buttons cycle through the images, and a counter element shows the current image position (e.g., "3 / 15"). The code-behind is empty — no server-side logic is involved. Accessible to everyone.

---

## Tips.aspx

**Code-behind:** `Tips.aspx.cs` | **Master page:** `Site1.Master`

A static tips and tricks page presenting curated cooking advice. Content is divided into five themed sections with unordered lists: Prep and Knife Skills, Ingredient Hacks, Cooking Techniques, Flavor Boosters, and Baking Tips. Each section contains four practical tips. The code-behind is empty — there is no dynamic content. Accessible to everyone.

---

## Quiz.aspx

**Code-behind:** `Quiz.aspx.cs` | **Master page:** `Site1.Master`

An interactive cooking knowledge quiz. On first load the page loads all questions from `QuizHelper.LoadQuizQuestions()`, randomises their order using `Guid.NewGuid()` as the sort key, and for each question shuffles the answer options while tracking where the correct answer ended up after the shuffle. The shuffled question list is stored in `Session["Questions"]` so it survives postbacks. The quiz markup renders all questions and their radio-button answer options directly in the page using an inline `for` loop. Two server-side buttons drive the flow: the Start button records `DateTime.Now` in `Session["QuizStart"]` and sets a hidden field (`restoreQuizState`) to signal the client-side JavaScript (`quiz.js`) to reveal the quiz section after the postback. The Submit button iterates every question, compares the user's selected radio value against the correct answer text, and stores a `Dictionary<int, bool>` of question-number-to-correctness in `Session["QuizResults"]`. It then calculates elapsed time and redirects to `QuizResults.aspx`. The username is shown in the heading if logged in. Authentication is currently not enforced (the redirect guard is commented out).

---

## QuizResults.aspx

**Code-behind:** `QuizResults.aspx.cs` | **Master page:** `Site1.Master`

The quiz results page, displayed immediately after submitting the quiz. It reads `Session["Questions"]` and `Session["QuizResults"]` to calculate the final score as a percentage. The page renders: an SVG progress ring whose stroke-dashoffset is calculated from the score percentage and colour-coded (green above 75%, amber above 50%, orange above 25%, red otherwise); a text label ("Master Chef!", "Good job!", "Could be better…", or "Try again") inside the ring; and three stat cards showing the count of correct answers, wrong answers, and time taken. Below the stats, every question is listed as a card showing the question text and a "Correct" or "Wrong" tag. Time is displayed as a formatted `mm:ss` string stored in `Session["QuizTimer"]`. Authentication is not currently enforced (guard is commented out).

---

## Admin/ViewUsers.aspx

**Code-behind:** `Admin/ViewUsers.aspx.cs` | **Master page:** `Admin/AdminSidebar.master`

A read-only admin page that displays a table of all registered users. On every page load it queries `SELECT * FROM Users` using a `SqlDataAdapter` and `DataSet`. It then iterates the result set's columns and rows to build an HTML table string, skipping the `Password` column entirely so that passwords are never exposed to the admin viewer. The resulting HTML is injected into a `div` element via `adminDiv.InnerHtml`. If the database query fails, an error message is shown instead. This page is intended for admin use; there is no server-side access guard enforced in the code.

---

## Admin/ManageUsers.aspx

**Code-behind:** `Admin/ManageUsers.aspx.cs` | **Master page:** `Admin/AdminSidebar.master`

A full CRUD admin page for managing user accounts. On load, `BindUsers()` queries the database and builds an HTML table of users (ID, Username, Birthday, Gender, Cuisine, Skill) with Edit and Delete buttons for each row. The Edit and Delete buttons call JavaScript functions (`openEdit`, `confirmDelete`) that populate a sliding form panel or a confirmation dialog respectively. The form panel is used for both creating new users and editing existing ones; a hidden `HiddenUserId` field distinguishes the two modes. Three hidden `asp:Button` controls (`BtnCreate`, `BtnEdit`, `BtnDelete`) are triggered programmatically by `manage-users.js` to cause a server postback for each operation. The corresponding server-side handlers (`BtnCreate_Click`, `BtnEdit_Click`, `BtnDelete_Click`) execute the appropriate SQL INSERT, UPDATE (password field is omitted from the UPDATE if left blank), or DELETE, then call `BindUsers()` to refresh the table and display a status message via `ShowMessage()`. Intended for admin use only; no server-side access guard is present in the current code.

---

## Admin/Statistics.aspx

**Code-behind:** `Admin/Statistics.aspx.cs` | **Master page:** `Admin/AdminSidebar.master`

A simple admin dashboard showing live site statistics. The page renders a two-column HTML table with two rows: "Online" (the value of `Application["Online"]`, the count of all active sessions) and "Logged In" (the value of `Application["LoggedIn"]`, the count of authenticated user sessions). Both counters are maintained by `Global.asax.cs`. The code-behind is empty — values are read directly in the markup via `<%= %>` expressions. Intended for admin use; no access guard in the current code.

---

## Guides/MeatGuide.aspx

**Code-behind:** `Guides/MeatGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide on cooking meat. The page covers: an introduction explaining how heat affects meat proteins, collagen, and fat; a Preparation section with advice on trimming, marinating, dry-brining, and tying roasts; a Tips section listing ten detailed, title-annotated tips (room temperature, drying the surface, salting early, using a thermometer, reverse sear, resting, cutting against the grain, matching method to cut, not crowding the pan, using fond); and an Internal Temperatures reference table covering five doneness levels (Rare through Well Done) in both Celsius and Fahrenheit with texture descriptions. The code-behind is empty — all content is static markup. Accessible to everyone.

---

## Guides/PoultryGuide.aspx

**Code-behind:** `Guides/PoultryGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide covering the preparation and cooking of poultry (chicken, turkey, etc.). Like all guide pages, it follows the same structure (introduction, preparation advice, tips list, reference table) and has an empty code-behind. Accessible to everyone.

---

## Guides/FishGuide.aspx

**Code-behind:** `Guides/FishGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide on cooking fish and seafood. Follows the standard guide layout with preparation notes, technique tips, and a reference table. Empty code-behind. Accessible to everyone.

---

## Guides/VegetablesGuide.aspx

**Code-behind:** `Guides/VegetablesGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide on preparing and cooking vegetables. Covers selection, preparation, and cooking methods for common vegetables. Follows the standard guide layout with an empty code-behind. Accessible to everyone.

---

## Guides/EggsGuide.aspx

**Code-behind:** `Guides/EggsGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide dedicated to eggs and the various ways to cook them (boiling, poaching, frying, scrambling, etc.). Follows the standard guide layout with an empty code-behind. Accessible to everyone.

---

## Guides/PastaGuide.aspx

**Code-behind:** `Guides/PastaGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide on cooking pasta, covering dough, water ratios, cooking times, and sauce pairing principles. Follows the standard guide layout with an empty code-behind. Accessible to everyone.

---

## Guides/BakingGuide.aspx

**Code-behind:** `Guides/BakingGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide covering bread and baking. Introduces the precision required in baking and covers: a Preparation section on weighing, temperature of ingredients, oven preheating, proofing, and scoring; a Tips section with seven detailed tips (weighing, not overmixing, steam for crusty bread, the windowpane test, testing cakes with a skewer, cooling before cutting, the role of fat); and an Oven Temperature Guide table listing recommended temperatures in Celsius and Fahrenheit for seven common baked goods (artisan bread, baguettes, sandwich loaves, layer cakes, muffins, shortcrust pastry, cookies) with notes on technique. The code-behind is empty. Accessible to everyone.

---

## Guides/SoupsGuide.aspx

**Code-behind:** `Guides/SoupsGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide on making soups, covering stock preparation, layering flavours, and finishing techniques. Follows the standard guide layout with an empty code-behind. Accessible to everyone.

---

## Guides/SaucesGuide.aspx

**Code-behind:** `Guides/SaucesGuide.aspx.cs` | **Master page:** `Guides/GuideSidebar.Master`

A static educational guide on making sauces, including emulsification, reduction, and the five mother sauces. Follows the standard guide layout with an empty code-behind. Accessible to everyone.

---

## Utilities/Timer.aspx

**Code-behind:** `Utilities/Timer.aspx.cs` | **Master page:** `Utilities/UtilitiesSidebar.Master`

A client-side cooking countdown timer utility. The page renders a single card with an editable time display divided into hours, minutes, and seconds segments; clicking a segment while the timer is idle opens an inline number input to change that segment's value. A single Start/Stop button toggles the timer on and off, and a thin progress bar at the bottom of the card fills as time elapses. All timer logic is handled entirely in `timer.js` — the code-behind is empty. The timer persists only in the browser (no server state). Accessible to everyone.

---

## Utilities/UnitsCalculator.aspx

**Code-behind:** `Utilities/UnitsCalculator.aspx.cs` | **Master page:** `Utilities/UtilitiesSidebar.Master`

A client-side measurement conversion utility for use while following recipes. The page is divided into two panels, switched by a tab bar: Volume and Mass/Weight. Each panel has an amount input and a "from unit" dropdown. When the user types a number and selects a unit, `units-calculator.js` calculates conversions to all other units in the same category and injects result cards into the results grid. Volume supports nine units (ml, tsp, tbsp, fl oz, cup, pint, quart, gallon, litre) using US customary definitions. Mass supports five units (mg, g, kg, oz, lb) with exact metric-imperial equivalents. No server-side logic is used — the code-behind is empty. Accessible to everyone.
