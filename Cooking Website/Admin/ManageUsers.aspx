<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSidebar.master"
    AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs"
    Inherits="Cooking_Website.Admin.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">Manage Users</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">Manage Users</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="/css/form.css" />
    <script type="text/javascript">
        var ManageUsersConfig = {
            hiddenUserIdId: "<%= HiddenUserId.ClientID %>",
            btnCreateId:    "<%= BtnCreate.ClientID %>",
            btnEditId:      "<%= BtnEdit.ClientID %>",
            btnDeleteId:    "<%= BtnDelete.ClientID %>"
        };
    </script>
    <script src="/js/manage-users.js" defer></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Body" runat="server">
    <button class="btn manage-add-btn" type="button" onclick="openCreate()">+ Add User</button>

    <!-- ── Server Message ── -->
    <asp:Label ID="lblMessage" runat="server" CssClass="manage-server-msg"></asp:Label>

    <!-- ── Users Table (built by codebehind like ViewUsers) ── -->
    <div class="manage-table-wrapper">
        <div id="adminDiv" runat="server"></div>
        <asp:Label ID="errorMsg" runat="server" CssClass="manage-error"></asp:Label>
    </div>

    <!-- ── Create / Edit Form Panel ── -->
    <div class="form-panel" id="form-panel" hidden>
        <div class="form-container manage-form">

            <div class="manage-form-header">
                <h2 class="form-title" id="form-panel-title">Create User</h2>
                <button class="panel-close-btn" type="button" onclick="closePanel()">&#10005;</button>
            </div>
            <p class="form-subtitle" id="form-panel-subtitle">Fill in the details for the new user.</p>

            <!-- Holds the user Id in edit mode -->
            <asp:HiddenField ID="HiddenUserId" runat="server" Value="0" />

            <label for="uname" class="lbl req">Username</label>
            <input type="text" id="uname" oninput="checkName()" autocomplete="off" />
            <div id="uname-msg" class="msg"></div>

            <label for="pwd" class="lbl" id="pwd-label">Password</label>
            <input type="password" id="pwd" oninput="checkPass()" autocomplete="new-password" />
            <div id="pwd-msg" class="msg"></div>

            <label for="pwd-valid" class="lbl" id="pwd-valid-label">Verify Password</label>
            <input type="password" id="pwd-valid" oninput="checkPassValidation()" />
            <div id="pwd-valid-msg" class="msg"></div>

            <label for="bday" class="lbl req">Birthday</label>
            <input type="date" id="bday" onchange="checkBirthday()" />
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

            <label class="lbl req">Favourite Cuisine</label>
            <div class="checkbox-group">
                <label>
                    <input type="checkbox" name="cuisine" value="italian" onchange="checkCuisine()" />
                    Italian</label>
                <label>
                    <input type="checkbox" name="cuisine" value="indian" onchange="checkCuisine()" />
                    Indian</label>
                <label>
                    <input type="checkbox" name="cuisine" value="japanese" onchange="checkCuisine()" />
                    Japanese</label>
                <label>
                    <input type="checkbox" name="cuisine" value="mediterranean" onchange="checkCuisine()" />
                    Mediterranean</label>
                <label>
                    <input type="checkbox" name="cuisine" value="mexican" onchange="checkCuisine()" />
                    Mexican</label>
                <label>
                    <input type="checkbox" name="cuisine" value="american" onchange="checkCuisine()" />
                    American</label>
                <label>
                    <input type="checkbox" name="cuisine" value="chinese" onchange="checkCuisine()" />
                    Chinese</label>
                <label>
                    <input type="checkbox" name="cuisine" value="french" onchange="checkCuisine()" />
                    French</label>
                <label>
                    <input type="checkbox" name="cuisine" value="other" onchange="checkCuisine()" />
                    Other</label>
            </div>
            <div id="cuisine-msg" class="msg"></div>

            <label for="skill" class="lbl req">Cooking Skill Level</label>
            <select id="skill" onchange="checkSkill()">
                <option value="">-- Select Skill Level --</option>
                <option value="beginner">Beginner</option>
                <option value="intermediate">Intermediate</option>
                <option value="expert">Expert</option>
            </select>
            <div id="skill-msg" class="msg"></div>

            <p class="required-note"><span>*</span> Mandatory fields</p>

            <button class="btn" type="button" onclick="onSubmit()">Save</button>
            <button class="btn secondary" type="button" onclick="closePanel()">Cancel</button>

        </div>
    </div>

    <!-- ── Delete Confirm Panel ── -->
    <div class="delete-panel" id="delete-panel" hidden>
        <div class="delete-confirm-box">
            <p class="delete-confirm-msg" id="delete-confirm-msg"></p>
            <div class="delete-confirm-actions">
                <button class="btn delete-confirm-btn" type="button" onclick="submitDelete()">Delete</button>
                <button class="btn secondary" type="button" onclick="closeDeletePanel()">Cancel</button>
            </div>
        </div>
    </div>

    <!-- Hidden buttons — clicked by JS to trigger server postbacks -->
    <asp:Button ID="BtnCreate" runat="server" Style="display: none" OnClick="BtnCreate_Click" />
    <asp:Button ID="BtnEdit" runat="server" Style="display: none" OnClick="BtnEdit_Click" />
    <asp:Button ID="BtnDelete" runat="server" Style="display: none" OnClick="BtnDelete_Click" />
</asp:Content>
