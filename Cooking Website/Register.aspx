<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Cooking_Website.Register" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/form.css" />
    <script src="/js/register.js" defer></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">

    <div class="form-container">
        <h2 class="form-title">Create Your Account</h2>
        <p class="form-subtitle">Join the FreshBite community</p>

        <label for="uname" class="lbl req">Username</label>
        <input type="text" name="uname" id="uname" oninput="checkName()" />
        <div id="uname-msg" class="msg"></div>

        <label for="pwd" class="lbl req">Password</label>
        <input type="password" name="pwd" id="pwd" oninput="checkPass()" />
        <div id="pwd-msg" class="msg"></div>

        <label for="pwd-valid" class="lbl req">Verify Password</label>
        <input type="password" id="pwd-valid" oninput="checkPassValidation()" />
        <div id="pwd-valid-msg" class="msg"></div>

        <label for="bday" class="lbl req">Birthday</label>
        <input type="date" name="bday" id="bday" onchange="checkBirthday()" />
        <div id="bday-msg" class="msg"></div>

        <label class="lbl req">Gender</label>
        <div class="radio-group">
            <label>
                <input type="radio" name="gender" value="male" oninput="checkGender()" />
                Male</label>
            <label>
                <input type="radio" name="gender" value="female" oninput="checkGender()" />
                Female</label>
        </div>
        <div id="gender-msg" class="msg"></div>

        <label for="cuisine" class="lbl req">Favorite Cuisine</label>
        <div class="checkbox-group" id="cuisine">
            <label>
                <input type="checkbox" id="cuisine-italian" name="cuisine" value="italian" onchange="checkCuisine()" />
                Italian
            </label>
            <label>
                <input type="checkbox" id="cuisine-indian" name="cuisine" value="indian" onchange="checkCuisine()" />
                Indian
            </label>
            <label>
                <input type="checkbox" id="cuisine-japanese" name="cuisine" value="japanese" onchange="checkCuisine()" />
                Japanese
            </label>
            <label>
                <input type="checkbox" id="cuisine-mediterranean" name="cuisine" value="mediterranean" onchange="checkCuisine()" />
                Mediterranean
            </label>
            <label>
                <input type="checkbox" id="cuisine-mexican" name="cuisine" value="mexican" onchange="checkCuisine()" />
                Mexican
            </label>
        </div>
        <div id="cuisine-msg" class="msg"></div>

        <label for="skill" class="lbl req">Cooking Skill Level</label>
        <select id="skill" name="skill" onchange="checkSkill()">
            <option value="">-- Select Skill Level --</option>
            <option value="beginner">Beginner</option>
            <option value="intermediate">Intermediate</option>
            <option value="expert">Expert</option>
        </select>
        <div id="skill-msg" class="msg"></div>

        <div class="checkbox-group">
            <label>
                <input type="checkbox" id="terms" name="terms" value="yes" oninput="checkTerms()" />
                <span class="req">Agree to our terms of use</span>
            </label>
        </div>
        <div id="terms-msg" class="msg"></div>

        <p class="required-note"><span>*</span> Mandatory fields</p>

        <input type="submit" class="btn" onclick="return onSubmit()" value="Register" />
        <input type="reset" class="btn secondary" onclick="onReset()" value="Reset" />

        <p class="form-footer">
            Already have an account? <a href="Login.aspx">Sign in</a>.
   
        </p>

        <asp:Label ID="lblMessage" runat="server" CssClass="server-msg"></asp:Label>
    </div>

    

</asp:Content>
