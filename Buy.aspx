<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Buy.aspx.vb" Inherits="Buy" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            text-align: center;
        }
    </style>
</asp:content>
<asp:Content ID="body" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Are you sure you want to buy <u><asp:Label ID="lblName" runat="server"></asp:Label></u> for <u><asp:Label ID="lblPrice" runat="server"></asp:Label></u> dC?</h2>
    <asp:LinkButton ID="btnBuy" runat="server"><div id="divBuy" style="margin: auto; height: 30px; width: 100px; font-size: 20px; background-color: darkseagreen; color: black; padding: 10px; border-radius: 5px;">Buy!</div></asp:LinkButton>
    <br /><br />
    <asp:LinkButton ID="btnCancel" runat="server"><div id="divCancel" style="margin: auto; height: 30px; width: 100px; font-size: 20px; background-color: indianred; color: black; padding: 10px; border-radius: 5px;">No.</div></asp:LinkButton>
</asp:Content>