<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Cooking_Website.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/profile.css" />
    <script src="/js/profile.js" defer></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="profile-container">
        <div class="profile-card">
            <div class="card-header">
                <div class="header-left">
                    <div class="avatar" id="avatar-initials"><%=Session["Username"].ToString().Substring(0,2).ToUpper()%></div>
                    <div>
                        <h2>My Profile</h2>
                        <p id="header-username">@<%=Session["Username"]%></p>
                    </div>
                </div>
                <button type="button" class="edit-btn" id="toggle-btn" onclick="toggleEdit()">Edit</button>
            </div>

            <div class="card-body">

                <%-- Each field has a view element and a matching edit element; profile.js toggles between them --%>
                <div class="field-row">
                    <span class="field-label">Username</span>
                    <div class="field-value" id="view-username"><%= Session["Username"] %></div>
                    <input class="field-input" id="edit-username" name="username" type="text" value="<%= Session["Username"] %>" style="display: none">
                    <span class="field-error" id="error-username"></span>
                </div>

                <div class="field-row">
                    <span class="field-label">Password</span>
                    <div class="field-value password-dots" id="view-password">••••••••</div>
                    <div id="edit-password" style="display: none; flex-direction: column; gap: 8px;">
                        <input class="field-input" id="edit-current-password" name="currentPassword" type="password" placeholder="Current password">
                        <span class="field-error" id="error-current-password"></span>
                        <input class="field-input" id="edit-new-password" name="newPassword" type="password" placeholder="New password">
                        <span class="field-error" id="error-new-password"></span>
                        <input class="field-input" id="edit-confirm-password" name="confirmPassword" type="password" placeholder="Confirm new password">
                        <span class="field-error" id="error-confirm-password"></span>
                    </div>
                </div>

                <div class="divider"></div>

                <%-- Parse Birthday once so both view (dd/MM/yyyy) and edit (yyyy-MM-dd) can use it --%>
                <% DateTime birthday = DateTime.Parse(profileInfo["Birthday"].ToString()); %>
                <div class="field-row">
                    <span class="field-label">Birthday</span>
                    <div class="field-value" id="view-birthday"><%=birthday.ToString("dd/MM/yyyy") %></div>
                    <input class="field-input" id="edit-birthday" name="birthday" type="date" value="<%= birthday.ToString("yyyy-MM-dd")%>" style="display: none">
                    <span class="field-error" id="error-birthday"></span>
                </div>

                <%
                    string currentGender = profileInfo["Gender"].ToString().ToLower();
                    string[] genderOptions = { "male", "female" };
                %>
                <div class="field-row">
                    <span class="field-label">Gender</span>
                    <div class="field-value" id="view-gender"><%=char.ToUpper(currentGender[0]) + currentGender.Substring(1)%></div>
                    <select class="field-input" id="edit-gender" name="gender" style="display: none">
                        <% foreach (string g in genderOptions)
                            { %>
                        <option value="<%=g%>" <%=currentGender == g ? "selected" : ""%>><%=char.ToUpper(g[0]) + g.Substring(1)%></option>
                        <% } %>
                    </select>
                    <span class="field-error" id="error-gender"></span>
                </div>

                <div class="divider"></div>

                <%
                    string[] cuisines = (string[])profileInfo["Cuisine"];
                    string[] cuisineOptions = { "italian", "japanese", "mexican", "indian", "french", "chinese", "mediterranean", "american", "other" };
                %>
                <div class="field-row">
                    <span class="field-label">Favorite Cuisine</span>
                    <div class="multi-pills" id="view-cuisine">
                        <% foreach (string c in cuisines)
                            { %>
                        <span class="multi-pill"><%=char.ToUpper(c.Trim()[0]) + c.Trim().Substring(1).ToLower()%></span>
                        <% } %>
                    </div>
                    <div class="multi-options" id="edit-cuisine" style="display: none">
                        <% foreach (string option in cuisineOptions)
                            {
                                bool isChecked = ((IEnumerable<string>)cuisines).Any(c => c.Trim().ToLower() == option);
                        %>
                        <label class="multi-option">
                            <input type="checkbox" name="cuisine" value="<%=option%>" <%=isChecked ? "checked" : ""%>>
                            <%=char.ToUpper(option[0]) + option.Substring(1)%>
                        </label>
                        <% } %>
                    </div>
                    <span class="field-error" id="error-cuisine"></span>
                </div>

                <% string skill = profileInfo["Skill"].ToString(); %>
                <div class="field-row">
                    <span class="field-label">Cooking Skill</span>
                    <div class="field-value" id="view-skill"><%=char.ToUpper(skill[0]) + skill.Substring(1)%></div>
                    <select class="field-input" id="edit-skill" name="skill" style="display: none">
                        <option value="beginner" <%=skill == "beginner" ? "selected" : "" %>>Beginner</option>
                        <option value="intermediate" <%=skill == "intermediate" ? "selected" : "" %>>Intermediate</option>
                        <option value="expert" <%=skill == "expert" ? "selected" : "" %>>Expert</option>
                    </select>
                    <span class="field-error" id="error-skill"></span>
                </div>
                <asp:Label ID="serverMsg" runat="server" CssClass="server-message"></asp:Label>

            </div>

            <div class="card-footer" id="card-footer">
                <button type="button" class="cancel-btn" onclick="cancelEdit()">Cancel</button>
                <%-- saveEdit() runs client-side validation first; returns false to abort the postback on error --%>
                <asp:Button ID="saveBtn" runat="server" Text="Save Changes" CssClass="save-confirm-btn" OnClick="saveBtn_Click" OnClientClick="return saveEdit()" />
            </div>
        </div>
    </div>
</asp:Content>
