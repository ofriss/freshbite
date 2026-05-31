<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSidebar.master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Cooking_Website.Admin.Statistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">Statistics</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">Statistics</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">
    <table>
        <thead>
            <tr>
                <th>Statistic</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <%-- Application-level counters managed by Global.asax.cs --%>
            <tr>
                <td>Online</td>
                <td><%=Application["Online"] %></td>
            </tr>
            <tr>
                <td>Logged In</td>
                <td><%=Application["LoggedIn"] %></td>
            </tr>
        </tbody>
    </table>
</asp:Content>