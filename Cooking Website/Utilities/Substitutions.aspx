<%@ Page Title="" Language="C#" MasterPageFile="~/Utilities/UtilitiesSidebar.Master" AutoEventWireup="true" CodeBehind="Substitutions.aspx.cs" Inherits="Cooking_Website.Substitutions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="/css/substitutions.css" />
    <script src="/js/substitutions.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <main class="main-container">

        <div class="page-title-block">
            <h1>Ingredient Substitutions</h1>
            <p>Common swaps for baking and cooking &ndash; with ratios.</p>
        </div>

        <%-- Live search input; substitutions.js filters table rows on every input event --%>
        <input class="sub-search" id="sub-search" type="search"
               placeholder="Search ingredient or substitute..." autocomplete="off" />

        <table class="sub-table" id="sub-table">
            <thead>
                <tr>
                    <th>Ingredient</th>
                    <th>Substitute</th>
                    <th>Ratio / Notes</th>
                    <th>Category</th>
                </tr>
            </thead>
            <tbody>
                <!-- Fats -->
                <tr>
                    <td>Butter</td>
                    <td>Vegetable Oil</td>
                    <td>Use 3/4 the amount (e.g. 1 cup butter &rarr; 3/4 cup oil)</td>
                    <td><span class="badge badge-fats">Fats</span></td>
                </tr>
                <tr>
                    <td>Butter</td>
                    <td>Coconut Oil</td>
                    <td>Equal amount (1:1); adds slight coconut flavour</td>
                    <td><span class="badge badge-fats">Fats</span></td>
                </tr>
                <tr>
                    <td>Butter</td>
                    <td>Applesauce</td>
                    <td>Equal amount; reduces fat, adds moisture &ndash; best in cakes/muffins</td>
                    <td><span class="badge badge-fats">Fats</span></td>
                </tr>
                <tr>
                    <td>Shortening</td>
                    <td>Butter</td>
                    <td>Equal amount + a pinch of salt</td>
                    <td><span class="badge badge-fats">Fats</span></td>
                </tr>

                <!-- Dairy -->
                <tr>
                    <td>Buttermilk</td>
                    <td>Milk + White Vinegar</td>
                    <td>1 cup milk + 1 tbsp vinegar; let sit 5 min before using</td>
                    <td><span class="badge badge-dairy">Dairy</span></td>
                </tr>
                <tr>
                    <td>Buttermilk</td>
                    <td>Milk + Lemon Juice</td>
                    <td>1 cup milk + 1 tbsp lemon juice; let sit 5 min</td>
                    <td><span class="badge badge-dairy">Dairy</span></td>
                </tr>
                <tr>
                    <td>Sour Cream</td>
                    <td>Greek Yogurt</td>
                    <td>Equal amount (1:1); full-fat works best</td>
                    <td><span class="badge badge-dairy">Dairy</span></td>
                </tr>
                <tr>
                    <td>Heavy Cream</td>
                    <td>Milk + Butter</td>
                    <td>3/4 cup milk + 1/4 cup melted butter per cup of cream</td>
                    <td><span class="badge badge-dairy">Dairy</span></td>
                </tr>
                <tr>
                    <td>Cream Cheese</td>
                    <td>Ricotta</td>
                    <td>Equal amount; texture will be softer and less tangy</td>
                    <td><span class="badge badge-dairy">Dairy</span></td>
                </tr>

                <!-- Eggs -->
                <tr>
                    <td>Egg</td>
                    <td>Ground Flaxseed</td>
                    <td>1 tbsp flaxseed + 3 tbsp water per egg; let sit 5 min</td>
                    <td><span class="badge badge-eggs">Eggs</span></td>
                </tr>
                <tr>
                    <td>Egg</td>
                    <td>Unsweetened Applesauce</td>
                    <td>1/4 cup per egg; best in dense baked goods</td>
                    <td><span class="badge badge-eggs">Eggs</span></td>
                </tr>
                <tr>
                    <td>Egg</td>
                    <td>Ripe Banana</td>
                    <td>1/4 cup mashed banana per egg; adds banana flavour</td>
                    <td><span class="badge badge-eggs">Eggs</span></td>
                </tr>
                <tr>
                    <td>Egg</td>
                    <td>Chia Seeds</td>
                    <td>1 tbsp chia + 3 tbsp water per egg; let sit 10 min</td>
                    <td><span class="badge badge-eggs">Eggs</span></td>
                </tr>

                <!-- Sweeteners -->
                <tr>
                    <td>White Sugar</td>
                    <td>Honey</td>
                    <td>Use 3/4 amount; reduce other liquids by 1/4 cup per cup of honey used</td>
                    <td><span class="badge badge-sweet">Sweeteners</span></td>
                </tr>
                <tr>
                    <td>White Sugar</td>
                    <td>Brown Sugar</td>
                    <td>Equal amount (1:1); adds a mild molasses flavour</td>
                    <td><span class="badge badge-sweet">Sweeteners</span></td>
                </tr>
                <tr>
                    <td>White Sugar</td>
                    <td>Maple Syrup</td>
                    <td>Use 3/4 amount; reduce other liquids by 3 tbsp per cup</td>
                    <td><span class="badge badge-sweet">Sweeteners</span></td>
                </tr>
                <tr>
                    <td>Brown Sugar</td>
                    <td>White Sugar + Molasses</td>
                    <td>1 cup white sugar + 1 tbsp molasses, mixed well</td>
                    <td><span class="badge badge-sweet">Sweeteners</span></td>
                </tr>
                <tr>
                    <td>Powdered Sugar</td>
                    <td>White Sugar + Cornstarch</td>
                    <td>Blend 1 cup sugar + 1 tbsp cornstarch until fine</td>
                    <td><span class="badge badge-sweet">Sweeteners</span></td>
                </tr>

                <!-- Flour -->
                <tr>
                    <td>All-Purpose Flour</td>
                    <td>Whole Wheat Flour</td>
                    <td>Use 7/8 the amount (7/8 cup per cup); adds density</td>
                    <td><span class="badge badge-flour">Flour</span></td>
                </tr>
                <tr>
                    <td>Cake Flour</td>
                    <td>All-Purpose Flour</td>
                    <td>1 cup minus 2 tbsp all-purpose per cup of cake flour</td>
                    <td><span class="badge badge-flour">Flour</span></td>
                </tr>
                <tr>
                    <td>Self-Rising Flour</td>
                    <td>All-Purpose + Leavening</td>
                    <td>1 cup all-purpose + 1 1/2 tsp baking powder + 1/4 tsp salt</td>
                    <td><span class="badge badge-flour">Flour</span></td>
                </tr>

                <!-- Leavening -->
                <tr>
                    <td>Baking Powder (1 tsp)</td>
                    <td>Baking Soda + Cream of Tartar</td>
                    <td>1/4 tsp baking soda + 1/2 tsp cream of tartar</td>
                    <td><span class="badge badge-leaven">Leavening</span></td>
                </tr>
                <tr>
                    <td>Baking Soda (1 tsp)</td>
                    <td>Baking Powder</td>
                    <td>3 tsp baking powder (3x the amount)</td>
                    <td><span class="badge badge-leaven">Leavening</span></td>
                </tr>
                <tr>
                    <td>Instant Yeast</td>
                    <td>Active Dry Yeast</td>
                    <td>Use 25% more; activate in warm water first</td>
                    <td><span class="badge badge-leaven">Leavening</span></td>
                </tr>

                <!-- Flavoring -->
                <tr>
                    <td>Vanilla Extract (1 tsp)</td>
                    <td>Vanilla Bean</td>
                    <td>Scrape seeds from 1/2 a vanilla bean</td>
                    <td><span class="badge badge-flavor">Flavoring</span></td>
                </tr>
                <tr>
                    <td>Cocoa Powder (3 tbsp)</td>
                    <td>Dark Chocolate</td>
                    <td>1 oz dark chocolate + reduce fat in recipe by 1 tbsp</td>
                    <td><span class="badge badge-flavor">Flavoring</span></td>
                </tr>

                <!-- Thickeners -->
                <tr>
                    <td>Cornstarch</td>
                    <td>All-Purpose Flour</td>
                    <td>Use 2x the amount of flour (1 tbsp cornstarch = 2 tbsp flour)</td>
                    <td><span class="badge badge-thick">Thickeners</span></td>
                </tr>
                <tr>
                    <td>Cornstarch</td>
                    <td>Arrowroot Powder</td>
                    <td>Equal amount (1:1)</td>
                    <td><span class="badge badge-thick">Thickeners</span></td>
                </tr>
            </tbody>
        </table>

        <p class="sub-empty" id="sub-empty" style="display:none">No matches found.</p>

    </main>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">Substitutions</asp:Content>
