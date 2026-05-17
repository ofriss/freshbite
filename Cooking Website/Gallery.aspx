<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="Cooking_Website.Gallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/gallery.css" />
    <script src="/js/gallery.js" defer></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="main">
        <div class="controls">
            <button type="button" id="prev">← prev</button>
            <span class="counter" id="counter"></span>
            <button type="button" id="next">next →</button>
        </div>

        <div class="frame" id="frame">
            <img src="https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=1200&h=800&fit=crop&q=80" class="active" alt="Food 1" />
            <img src="https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=1200&h=800&fit=crop&q=80" alt="Food 2" />
            <img src="https://images.unsplash.com/photo-1544025162-d76694265947?w=1200&h=800&fit=crop&q=80" alt="Food 3" />
            <img src="https://images.unsplash.com/photo-1529193591184-b1d58069ecdd?w=1200&h=800&fit=crop&q=80" alt="Food 4" />
            <img src="https://images.unsplash.com/photo-1600891964092-4316c288032e?w=1200&h=800&fit=crop&q=80" alt="Food 5" />
            <img src="https://images.unsplash.com/photo-1558030006-450675393462?w=1200&h=800&fit=crop&q=80" alt="Food 6" />
            <img src="https://images.unsplash.com/photo-1571091718767-18b5b1457add?w=1200&h=800&fit=crop&q=80" alt="Food 7" />
            <img src="https://images.unsplash.com/photo-1567620832903-9fc6debc209f?w=1200&h=800&fit=crop&q=80" alt="Food 8" />
            <img src="https://images.unsplash.com/photo-1606728035253-49e8a23146de?w=1200&h=800&fit=crop&q=80" alt="Food 9" />
            <img src="https://images.unsplash.com/photo-1610057099443-fde8c4d50f91?w=1200&h=800&fit=crop&q=80" alt="Food 10" />
            <img src="https://images.unsplash.com/photo-1532550907401-a500c9a57435?w=1200&h=800&fit=crop&q=80" alt="Food 11" />
            <img src="https://images.unsplash.com/photo-1562802378-063ec186a863?w=1200&h=800&fit=crop&q=80" alt="Food 12" />
            <img src="https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=1200&h=800&fit=crop&q=80" alt="Food 13" />
            <img src="https://images.unsplash.com/photo-1617093727343-374698b1b08d?w=1200&h=800&fit=crop&q=80" alt="Food 14" />
            <img src="https://images.unsplash.com/photo-1592415499556-74fcb9f18667?w=1200&h=800&fit=crop&q=80" alt="Food 15" />
        </div>

        <div class="caption" id="caption"></div>
    </div>

</asp:Content>
