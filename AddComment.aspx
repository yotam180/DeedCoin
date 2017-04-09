<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AddComment.aspx.vb" Inherits="AddComment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        body {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>Add comment to <u><asp:Label ID="lblName" runat="server"></asp:Label></u></h2>
    <asp:TextBox ID="txtContent" runat="server" style="width: 40%; min-height: 100px; vertical-align: top;" TextMode="MultiLine" placeholder="Comment here..." CssClass="large_txt"></asp:TextBox><br /><br />
    <asp:Button ID="btnSend" runat="server" Text="Send comment!"/>
</asp:Content>

