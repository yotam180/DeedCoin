<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="DeleteProposal.aspx.vb" Inherits="DeleteProposal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        body {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Delete something</h1>
    <asp:DropDownList ID="ddlRemove" runat="server"></asp:DropDownList><br />
    <asp:Button ID="btnDelete" Text="Delete" runat="server" />
</asp:Content>

