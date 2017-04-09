<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Notifications.aspx.vb" Inherits="Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $("#ContentPlaceHolder1_lblNotifications").html($("#ContentPlaceHolder1_lblNotifications").html.replace(/\<br\>))
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 30%; text-align: center;"></td><td style="vertical-align: top;">
                <h1>Notifications</h1>
                <asp:Label ID="lblNotifications" runat="server"></asp:Label>
            </td><td style="width: 30%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>

