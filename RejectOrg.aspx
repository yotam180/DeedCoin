<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RejectOrg.aspx.vb" Inherits="RejectOrg" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="head">
    <style>
        body {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:Panel ID="pnlMain" runat="server">
        <h2>Are you sure you want to approve <i><u><b><asp:Label ID="lblName" runat="server"></asp:Label></b></u></i>?</h2>
        <asp:LinkButton ID="btnReject" runat="server">
            <div style="display: inline-block; background-color: darkseagreen; width: 100px; min-height: 50px; border-radius: 15px; vertical-align: middle; margin: 0 auto; line-height: 40px; cursor: pointer;">
                <b>Reject</b>
            </div>
        </asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server">
            <div style="display: inline-block; background-color: indianred; width: 100px; min-height: 50px; border-radius: 15px; vertical-align: middle; margin: 0 auto; line-height: 40px; cursor: pointer;">
                <b>No</b>
            </div>
        </asp:LinkButton>
    </asp:Panel>
    <asp:Panel id="pnlNotFound" runat="server" Visible ="false">
        <h2>This organization was not found... :(</h2>
    </asp:Panel>
</asp:Content>