<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RecoverPassword.aspx.vb" Inherits="RecoverPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 20%; text-align: center;"></td><td>
                <br />
                <h2>Lost your password?</h2>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="large_txt" style="width: 70%; font-size: 17px; text-align: center;" placeholder="Email" TabIndex="1" Font-Bold="True"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblState" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                        <br /><br />
                        If your Email is registered, we will send your password to your Email. <br />
                        <asp:LinkButton ID="btnRecover" runat="server" TabIndex="3">
                            <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Recover password</div>
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td><td style="width: 20%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>

