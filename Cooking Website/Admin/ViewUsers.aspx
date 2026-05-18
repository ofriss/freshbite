<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSidebar.master" AutoEventWireup="true" CodeBehind="ViewUsers.aspx.cs" Inherits="Cooking_Website.Admin.ViewUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">View Users</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">View Users</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">
    <div id="adminDiv" runat="server"></div>
    <asp:Label ID="errorMsg" runat="server"></asp:Label>
</asp:Content>
