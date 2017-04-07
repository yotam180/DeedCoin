<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MyZone.aspx.vb" Inherits="MyZone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #maintbl td {
            text-align: center;
            vertical-align: top;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table id="maintbl" style="width: 100%; table-layout: fixed"><tr>
        <td>
            <h2>My organizations</h2>
            <a href="AddOrganization.aspx">+New organization</a><br /><br />
            <asp:Label ID="lblOrgs" runat="server"></asp:Label>
        </td>
        <td>
            <h2>My orders</h2>
            <asp:Label ID="lblOrders" runat="server"></asp:Label>
        </td>
        <td>
            <h2>My jobs</h2>
            <asp:Label ID="lblJobs" runat="server"></asp:Label>
        </td>
        <td>
            <h2>My gigs/jobs</h2>
            <a href="AddGig.aspx">+New Gig</a>&nbsp;&nbsp;&nbsp;<a href="DeleteProposal.aspx?gig=1">-Delete Gig</a><br />
            <a href="AddJob.aspx">+New Job</a>&nbsp;&nbsp;&nbsp;<a href="DeleteProposal.aspx">-Delete Job</a><br /><br />
            <asp:Label ID="lblGigs" runat="server"></asp:Label>
        </td>
    </tr></table>
</asp:Content>

