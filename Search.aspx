<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Search.aspx.vb" Inherits="Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="width: 100%; text-align: center;">
        <h2>Looking for something?</h2>
        <asp:TextBox ID="txtSearch" style="font-size: 17px;" CssClass="large_txt" Width="300px" placeholder="Search text..." runat="server"></asp:TextBox><asp:Button ID="btnSearch" Text="Go!" Height="38px" BackColor="Gold" Width="60px" runat="server" />
        <table style="table-layout: fixed; width: 100%; height: 30px;"><tr>
            <td style="text-align: center;">
                <asp:LinkButton ID="btnUsers" runat="server">
                    <div style="margin: auto; width: 50%; background-color: <%=c1%>; padding: 5px; border-radius: 5px;">Users</div>
                </asp:LinkButton>
            </td>
            <td style="text-align: center;">
                <asp:LinkButton ID="btnOrgs" runat="server">
                    <div style="margin: auto; width: 50%; background-color: <%=c2%>; padding: 5px; border-radius: 5px;">Organizations</div>
                </asp:LinkButton>
            </td>
            <td style="text-align: center;">
                <asp:LinkButton ID="btnGigs" runat="server">
                    <div style="margin: auto; width: 50%; background-color: <%=c3%>; padding: 5px; border-radius: 5px;">Gigs</div>
                </asp:LinkButton>
            </td>
            <td style="text-align: center;">
                <asp:LinkButton ID="btnJobs" runat="server">
                    <div style="margin: auto; width: 50%; background-color: <%=c4%>; padding: 5px; border-radius: 5px;">Jobs</div>
                </asp:LinkButton>
            </td>
        </tr></table>
    </div>
    <br /><br />
    <div style="border: 2px solid black; width: 70%; margin: auto; min-height: 200px;">
        <asp:Label ID="lblRes" runat="server"></asp:Label>
    </div>
</asp:Content>

