<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UpdateComment.aspx.vb" Inherits="UpdateComment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        body {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2><asp:Label ID="lblName" runat="server"></asp:Label></h2>
    <asp:TextBox ID="txtContent" runat="server" style="width: 40%; min-height: 100px; vertical-align: top;" TextMode="MultiLine" placeholder="Comment here..." CssClass="large_txt"></asp:TextBox><br /><br />
    <asp:Button ID="btnSend" runat="server" Text="Edit comment!"/>
</asp:Content>