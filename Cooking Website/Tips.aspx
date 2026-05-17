<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Tips.aspx.cs" Inherits="Cooking_Website.Tips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/tips.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <p>In this page you will find useful tips and tricks to help you cook better and faster!</p>

    <div class="tip-section">
        <h2>🔪 Prep & Knife Skills</h2>
        <ul>
            <li>Place a damp towel under your cutting board to keep it steady.</li>
            <li>Freeze onions for 10 minutes before chopping to reduce tears.</li>
            <li>Use a spoon to peel ginger for less waste.</li>
            <li>Keep your knives sharp as it’s safer and faster.</li>
        </ul>
    </div>

    <div class="tip-section">
        <h2>🥦 Ingredient Hacks</h2>
        <ul>
            <li>Store herbs like flowers: stems in water, loosely covered.</li>
            <li>Revive wilted greens by soaking them in ice water.</li>
            <li>Microwave citrus for 10 seconds to release more juice.</li>
            <li>Add a pinch of salt to sweet dishes to enhance flavor.</li>
        </ul>
    </div>

    <div class="tip-section">
        <h2>🍳 Cooking Techniques</h2>
        <ul>
            <li>Preheat your pan properly as food should sizzle instantly.</li>
            <li>Salt in layers for deeper, more balanced flavor.</li>
            <li>Let meat rest before slicing so juices redistribute.</li>
            <li>Use pasta water to thicken and emulsify sauces.</li>
        </ul>
    </div>

    <div class="tip-section">
        <h2>🧂 Flavor Boosters</h2>
        <ul>
            <li>Toast spices to unlock aroma and depth.</li>
            <li>Add a splash of acid (lemon, vinegar) when a dish tastes flat.</li>
            <li>Finish dishes with fresh herbs added at the end.</li>
            <li>Use umami boosters like soy sauce, miso, or parmesan.</li>
        </ul>
    </div>

    <div class="tip-section">
        <h2>🧁 Baking Tips</h2>
        <ul>
            <li>Weigh ingredients for consistent results.</li>
            <li>Chill cookie dough for richer flavor and better texture.</li>
            <li>Use room‑temperature eggs and butter for fluffier cakes.</li>
            <li>Add a bit of cornstarch for softer cookies.</li>
        </ul>
    </div>

</asp:Content>
