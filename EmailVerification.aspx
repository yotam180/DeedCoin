<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="EmailVerification.aspx.vb" Inherits="EmailVerification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 20%; text-align: center;"></td><td>
                <br />
                <h2>One more step!</h2>
                Just enter the verification code that was emailed to you in this textbox:<br />
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="large_txt" style="width: 70%; font-size: 17px; text-align: center;" placeholder="Verification number" TabIndex="1" Font-Bold="True"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblState" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                        <br /><br />
                        <asp:LinkButton ID="btnRegister" runat="server" TabIndex="2">
                            <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Verify!</div>
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br /><br />
                * Email make take up to a few minutes to appear. Be patient.
            </td><td style="width: 20%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>

