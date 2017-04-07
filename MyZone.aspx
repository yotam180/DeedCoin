<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MyZone.aspx.vb" Inherits="MyZone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #maintbl td {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table id="maintbl" style="width: 100%; table-layout: fixed"><tr>
        <td>
            <h2>My organizations</h2>
            <asp:Label ID="lblOrgs" runat="server"></asp:Label>
        </td>
        <td>
            <h2>My orders</h2>
            <asp:Label ID="lblOrders" runat="server"></asp:Label>
        </td>
        <td>
            <h2>My jobs</h2>
            <asp:Label ID="lblJobs" runat="server"></asp:Label>
        </td>
        <td>
            <h2>My notifications</h2>
            <asp:Label ID="lblNotifications" runat="server"></asp:Label>
        </td>
    </tr></table>
</asp:Content>

