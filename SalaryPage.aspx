<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SalaryPage.aspx.vb" Inherits="SalaryPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlInitial" runat="server">
        <div style="border-radius: 10px; border: 1px solid gray; width: 100%; min-height: 200px;">
            <div style="width: 100%; text-align: center;"><h2>Purchase details</h2></div>
            <table border="1">
                <tr>
                    <td style="width: 20%;"><asp:Image ID="imgPrc" runat="server" style="height: auto; width: 100%;" /></td>
                    <td>
                        <h4>Purchase item: <u><asp:Label ID="lblName" runat="server"></asp:Label></u> - <u><asp:Label ID="lblPrice" runat="server"></asp:Label></u> dC</h4>
                        Bought on: <asp:Label ID="lblBDate" runat="server"></asp:Label><br /><br />
                        Offerer: <asp:Label ID="lblSeller" runat="server"></asp:Label><br /><br />
                        Worker: <asp:Label ID="lblBuyer" runat="server"></asp:Label><br /><br />
                        Amount: <asp:Label ID="lblAmount" runat="server"></asp:Label><br /><br />
                        Work done?: <asp:Label ID="lblWorkDone" runat="server"></asp:Label><br /><br />
                        Paid?: <asp:Label ID="lblPaid" runat="server"></asp:Label>
                        <h4><asp:Label ID="lblRating" runat="server" Text=""></asp:Label></h4>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlApproveWork" Visible="false" runat="server">
        <div style="border-radius: 10px; border: 1px solid gray; width: 100%; min-height: 200px;">
            <div style="width: 100%; text-align: center;"><h2>Approve your work</h2>
            <strong>Rate this seller: </strong><asp:RadioButtonList style="margin: auto;" ID="rating" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                <asp:ListItem Text="5" Value="5"></asp:ListItem>
            </asp:RadioButtonList>
                </div>
            <asp:LinkButton ID="btnApprove" runat="server"><div id="divBuy" style="text-align: center; margin: auto; height: 30px; width: 200px; font-size: 20px; background-color: darkseagreen; color: black; padding: 10px; border-radius: 5px;">Work done!</div></asp:LinkButton>
            <br /><br />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlApprovePayment" Visible="false" runat="server">
        <div style="border-radius: 10px; border: 1px solid gray; width: 100%; min-height: 200px;">
            <div style="width: 100%; text-align: center;"><h2>Approve payment</h2></div>
            <asp:LinkButton ID="btnPay" runat="server"><div id="divPay" style="text-align: center; margin: auto; height: 30px; width: 200px; font-size: 20px; background-color: darkseagreen; color: black; padding: 10px; border-radius: 5px;">Pay</div></asp:LinkButton>
            <br /><br />
        </div>
    </asp:Panel>
</asp:Content>

