<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Conversation.aspx.vb" Inherits="Conversation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            var objDiv = document.getElementById('divConvo');
            objDiv.scrollTop = objDiv.scrollHeight;
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="scriptManager1" runat="server"></asp:ScriptManager>
        <table style="table-layout: fixed; width: 100%; position: fixed; height: 90%; top: 10%; left: 0;">
            <tr>
                <td style="vertical-align: top; min-height: 300px; width: 25%; background-color: aliceblue;">
                    <h3>Contacts:</h3>
                    <asp:Label ID="lblContacts" runat="server"></asp:Label>
                </td>
                <td style="min-height: 300px; width: 75%; background-color: lightgray; vertical-align: top; padding: 10px;">
                    <div  style="width: 100%; text-align: center; height: 5%;">
                        <strong>Your conversation with <asp:Label Text="_name_" ID="lblName" runat="server"></asp:Label>:</strong>
                    </div>
                    <div id="divConvo" style="width: 100%; height: 80%; background-color: whitesmoke; overflow-y: scroll;">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </div>
                    <div style="width: 100%; height: 15%; background-color: gray; vertical-align: middle;">
                        <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" CssClass="large_txt" style="display: inline-block; height: 70%; width: 85%; float: left;"></asp:TextBox>
                        <asp:Button ID="btnSend" AutoPostBack="True" runat="server" Width="10%" Height="50%" style="float: right; display: inline-block; height: 100%" Text="Send!" />
                    </div>
                </td>
            </tr>
        </table>
</asp:Content>

