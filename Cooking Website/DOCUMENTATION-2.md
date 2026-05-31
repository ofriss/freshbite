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

## Index.aspx

The home page shows a hero section with the FreshBite name, a short tagline, a food photograph, and an "Explore Recipes" button. Below it, three feature cards link directly to the Cooking Guides, the Recipes collection, and the Kitchen Tools. The page is open to everyone. Signed-in users see their name in the hero heading; visitors see a call-to-action strip at the bottom of the page inviting them to register for personalised recipe suggestions.

---

## Login.aspx

The sign-in page for returning users, showing a username field and a password field under the heading "Welcome Back." Each field validates itself as you type and shows a message immediately if the input is too short or contains invalid characters, so problems are caught before you even click submit. Entering valid credentials takes you to the home page; wrong credentials show a red error message below the form. A link at the bottom leads to the registration page for new visitors. This page is open to everyone.

---

## Register.aspx

The account creation page, where new visitors fill in a username, password and confirmation, date of birth, gender, favourite cuisines (any combination from a fixed list of nine), and cooking skill level, then agree to the terms before submitting. Each field validates in real time and the form will not submit until every field passes. If the chosen username is already taken, a message asks for a different one. On success, the account is created, the user is signed in, and the site redirects to the home page. A link at the bottom goes to the Login page for returning members. This page is open to everyone.

---

## Profile.aspx

The profile page shows a card with the user's avatar (built from the first two letters of their username), their username as a handle, and all account details: username, password (shown as dots), birthday, gender, favourite cuisines as coloured pill tags, and skill level. Clicking Edit turns the fields into inputs — text fields, a date picker, a gender dropdown, cuisine checkboxes, and a skill dropdown. The password section expands into three fields requiring the current password as a security check before any change is accepted. Save applies only the fields that actually changed; Cancel discards all edits. This page is accessible to signed-in users only — anyone not logged in is redirected to the Login page.

---

## Recipes.aspx

The recipe browsing page shows all recipes as a card grid, with a filter bar at the top for narrowing by difficulty, category, and one or more cuisines. Filters combine, so selecting Easy + Italian + Pasta returns only recipes matching all three. Each card shows an image or placeholder, the category and difficulty tags, the title, a short description, total cook time, and serving count. Signed-in users whose skill level and preferred cuisines match a recipe see a "For You" badge on that card. Visitors see a dismissible banner reminding them that signing in enables personalised suggestions. The page is open to everyone.

---

## Recipe.aspx

The full recipe detail page, reached by clicking a recipe card. If the recipe ID in the URL is missing or invalid, the site redirects back to the recipe list. A valid recipe shows an optional hero photo, category/cuisine/difficulty tags, the title, a description, and a stats row with prep time, cook time, total time, and a servings scaler. The scaler's plus and minus buttons proportionally adjust all ingredient quantities on the page. Ingredients can be individually checked off as you gather them. The steps follow as a numbered list, and a Tips section appears only when the recipe has tips. A "For You" badge appears for signed-in users whose profile matches the recipe. The page is open to everyone.

---

## Gallery.aspx

A food photography showcase with 15 Unsplash photos. One large image fills the frame at a time, with Previous and Next buttons on either side and a counter in the middle (e.g. "3 / 15"). Clicking the buttons cycles through the collection one image at a time. There is nothing to configure — the page is open to everyone.

---

## Tips.aspx

A static collection of practical cooking advice grouped into five sections: Prep & Knife Skills, Ingredient Hacks, Cooking Techniques, Flavor Boosters, and Baking Tips, each with four concise bullet points. There is nothing to interact with. The page is open to everyone.

---

## Quiz.aspx

A cooking knowledge quiz. On arrival, a welcome heading (personalised with the username if signed in) and a Start button are shown. Clicking Start reveals all questions at once, each in its own card with four radio-button answer options. Answers can be changed freely until the Send button is clicked. A client-side check prevents submission if any question was left unanswered. Question order and answer order are randomised fresh on every new quiz. On submission, the site redirects to the Quiz Results page. The quiz is available to everyone.

---

## QuizResults.aspx

The results page, reached automatically after submitting the quiz. The centrepiece is an animated SVG progress ring whose arc fills to represent the score percentage and changes colour by tier: green above 75%, amber above 50%, orange above 25%, and red below that. Inside the ring a one-word verdict is shown. Three stat cards beside it display the number of correct answers, wrong answers, and time taken. Below, every question is listed with a green "Correct" or red "Wrong" label so you can review each one. This page is available to everyone.

---

## Admin/ViewUsers.aspx

A read-only admin page that queries the database and displays all registered user accounts in a table. Every column from the Users table becomes a column header except Password, which is always skipped. If the database query fails, an error message is shown in place of the table. This page is inside the admin area, which has its own sidebar navigation layout.

---

## Admin/ManageUsers.aspx

The full user management tool for administrators. The page shows a table of all users with Edit and Delete buttons per row, and an "+ Add User" button at the top. Clicking Edit slides open a form panel pre-filled with that user's details; leaving the password field blank keeps the existing password. Clicking Delete shows a confirmation prompt before removing the record. "+ Add User" opens the same panel in blank creation mode. After any save or delete the table refreshes and a status message confirms the outcome. This page is in the admin area with the same sidebar layout as other admin pages.

---

## Admin/Statistics.aspx

A simple dashboard showing two live counters: how many browser sessions are currently open on the site, and how many of those belong to signed-in users. Both numbers reflect the current server state on each page load. There is nothing to click. This page is inside the admin area.

---

## Guides/MeatGuide.aspx

An in-depth technique article on cooking meat. It covers how heat affects proteins, collagen, and fat in different cuts, then walks through preparation advice (trimming, marinating, dry-brining, tying roasts) and ten detailed tips (bringing meat to room temperature, drying before searing, the reverse-sear technique, resting, cutting against the grain, matching cooking method to cut, not crowding the pan, and using fond for a pan sauce). A reference table at the end lists internal temperatures for five doneness levels in both Celsius and Fahrenheit. The page is open to everyone.

---

## Guides/PoultryGuide.aspx

A technique guide focused on cooking poultry correctly without drying it out or undercooking it. The preparation section covers patting dry, air-drying whole birds, spatchcocking, wet and dry brining, and why raw poultry should never be washed. Eight tips follow, covering thermometer use, the different temperature targets for white versus dark meat, cold-pan skin technique for thighs, resting, stuffing risks, butterflying breasts, and making stock from the carcass. A reference table lists safe internal temperatures for five poultry types. The page is open to everyone.

---

## Guides/FishGuide.aspx

A guide on cooking fish and seafood confidently, centred on three principles: freshness, dryness, and restraint with heat. Preparation covers checking for pin bones, patting dry, scoring whole fish, preparing mussels and clams, and deveining shrimp. Seven tips address judging freshness, knowing when fish is done, not moving fish in the pan prematurely, skin-side-down cooking, using acid to finish, discarding shellfish that do not open, and using gentle poaching for delicate fish. A doneness reference table covers six fish and seafood types. The page is open to everyone.

---

## Guides/VegetablesGuide.aspx

A guide on choosing the right cooking method for different types of vegetables. Preparation advice covers washing and drying before roasting, cutting to a uniform size, blanching bitter greens, salting eggplant or zucchini to draw out moisture, and par-cooking dense roots before roasting. Seven tips address high-heat roasting, not overcrowding the pan, seasoning generously, the blanch-and-shock technique for green vegetables, timing aromatics, using acid to finish, and roasting cut-side down for caramelisation. A blanching time reference table covers seven common vegetables. The page is open to everyone.

---

## Guides/EggsGuide.aspx

A guide on working with eggs and dairy, describing eggs as capable of emulsifying, leavening, binding, enriching, and coating. Six tips cover using room-temperature eggs for baking, low-and-slow scrambled eggs, why fresh eggs poach better, avoiding a rolling boil when boiling, never leaving cream or milk unattended on the stove, and tempering eggs before combining with hot liquid. A cooking times table covers six egg preparation methods. The page is open to everyone.

---

## Guides/PastaGuide.aspx

A guide on cooking pasta and grains well, focusing on water ratio, heat control, and timing. Preparation covers salting pasta water, using a large pot, rinsing rice selectively, and toasting dry grains to deepen flavour. Seven tips address salting pasta water generously, cooking al dente and finishing in the sauce, saving pasta water before draining, never rinsing cooked pasta unless using it cold, matching pasta shape to sauce weight, the absorption method for fluffy rice, and stirring risotto consistently. A grain cooking reference table covers six grains with water ratios, times, and technique notes. The page is open to everyone.

---

## Guides/BakingGuide.aspx

A guide to bread and baking, framing it as the most precise discipline in the kitchen where ratios, temperatures, and technique matter from the start. Preparation stresses weighing ingredients, bringing fats and eggs to room temperature, fully preheating the oven, proofing bread in a warm draught-free spot, and scoring bread just before baking. Seven tips cover weighing ingredients, not overmixing, using steam for crusty bread, the windowpane test for bread dough, testing cakes with a skewer, cooling before cutting, and understanding how fat temperature determines texture. An oven temperature reference table covers seven baked goods in both Celsius and Fahrenheit. The page is open to everyone.

---

## Guides/SoupsGuide.aspx

A guide on building deeply flavoured soups and stews. It goes straight into eight substantial tips: starting with a proper soffritto, using good stock, seasoning in layers, simmering rather than boiling, browning meat before adding it, deglazing the pot after browning, accepting that soups improve the next day, and finishing with something bright such as lemon or vinegar. There is no reference table. The page is open to everyone.

---

## Guides/SaucesGuide.aspx

A guide on sauce-making built around three core techniques: reduction, emulsification, and thickening. Seven tips are presented: using reduction to concentrate flavour without starch, the butter finishing technique for a glossy sauce, seasoning only after reduction is complete, keeping temperature control in mind for emulsified sauces, cooking a roux properly before adding liquid, using acid to balance richness, and straining through a fine mesh sieve for a smooth result. There is no reference table. The page is open to everyone.

---

## Utilities/Timer.aspx

A countdown timer for use while cooking. The page shows a large time display split into hours, minutes, and seconds; clicking any segment opens a small input field so you can type the desired value. A Start button begins the countdown and changes to Stop to allow pausing. A progress bar along the bottom fills as time elapses. The timer runs entirely in the browser and resets if the page is refreshed. This utility is open to everyone.

---

## Utilities/UnitsCalculator.aspx

A measurement conversion tool for use while following a recipe. A tab bar at the top switches between two tabs, Volume and Mass & Weight. In the Volume tab, typing an amount and choosing a unit from a dropdown instantly fills a results grid showing the equivalent in every other supported unit (millilitres, teaspoons, tablespoons, fluid ounces, cups, pints, quarts, gallons, and litres). The Mass & Weight tab works the same way for milligrams, grams, kilograms, ounces, and pounds. All conversions happen instantly in the browser without a page reload. Volume conversions use US customary definitions; mass conversions use exact metric-to-imperial equivalents. This utility is open to everyone.
