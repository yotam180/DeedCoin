<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="_404.aspx.vb" Inherits="_404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        body {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<h1>Weew! You shouldn't be her...</h1>
    <h3>Just navigate safely back to the previous page and continue DeedCoining!</h3>--%>
    <img src="Images/_404.png" style="position: fixed; height: 92%; width: 90%; top: 8%; left: 5%;" />
</asp:Content>

