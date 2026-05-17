<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Cooking_Website.Login" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/form.css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">

    <div class="form-container">
        <h2 class="form-title">Welcome Back</h2>
        <p class="form-subtitle">Sign in to continue your cooking journey</p>

        <label for="uname" class="lbl">Username</label>
        <input type="text" name="uname" id="uname" oninput="checkName()" />
        <div id="uname-msg" class="msg"></div>

        <label for="pwd" class="lbl">Password</label>
        <input type="password" name="pwd" id="pwd" oninput="checkPass()" />
        <div id="pwd-msg" class="msg"></div>

        <input type="submit" class="btn" onclick="return onSubmit()" value="Login" />
        <input type="reset" class="btn secondary" onclick="onReset()" value="Reset" />

        <p class="form-footer">
            Don't have an account? <a href="Register.aspx">Create one</a>.
        </p>

        <asp:Label ID="lblMessage" runat="server" CssClass="server-msg"></asp:Label>
    </div>

    <script src="js/login.js" defer></script>

</asp:Content>
