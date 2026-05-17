<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminSidebar.master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Cooking_Website.Admin.Statistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
    <table>
        <tr>
            <th>Statistic</th>
            <th>Data</th>
        </tr>
        <tr>
            <td>Online</td>
            <td><%=Application["Online"] %></td>
        </tr>
        <tr>
            <td>Logged In</td>
            <td><%=Application["LoggedIn"] %></td>
        </tr>
    </table>
</asp:Content>
