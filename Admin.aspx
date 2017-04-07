<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Admin.aspx.vb" Inherits="Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #adminTbl td {
            border: 1px dotted gray;
        }
        .radioButtonList { list-style:none; margin: 0; padding: 0;}
        .radioButtonList.horizontal li { display: inline;}

        .radioButtonList label{
            display:inline;
        }
    </style>
    <script>
        function appr(id)
        {
            location.href = "ApproveOrg.aspx?org=" + id;
        }
        function rej(id) {
            location.href = "RejectOrg.aspx?org=" + id;
        }

        function setMod(id) {
            var sel = document.getElementById(id);
            $.ajax({
                url: "/Backend/UpdateStatus.aspx",
                data: { user: id.substring(4), level: sel.value },
                type: "POST",
                success: function (e) {
                    if (e !== "OK") {
                        alert(e);
                        location.reload();
                    }
                    else {
                        location.reload();
                    }
                },
                error: function (a, b, c) {
                    alert("Connection error");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%; min-height: 200px; table-layout: fixed;" border="0">
        <tr>
            <td style="width: 20%; font-size: 20px; font-family: Arial;">
                <ul>
                    <asp:LinkButton ID="btnMUsers" runat="server"><li>Manage users</li></asp:LinkButton>
                    <asp:LinkButton ID="btnMAdmins" runat="server"><li>Manage admins</li></asp:LinkButton>
                    <asp:LinkButton ID="btnMOrganization" runat="server"><li>Manage organizations</li></asp:LinkButton>
                </ul>
            </td>
            <td style="vertical-align: top;">
                <div style="width: 100%; position: relative; text-align: center;">
                    <h1>Welcome, Admin.</h1>
                    <asp:Panel ID="pnlUsers" runat="server">
                        <div style="width: 100%; min-height: 30px; background-color: antiquewhite;">
                            <asp:Label ID="lblUsers" runat="server"></asp:Label>
                            <br />
                            <div style="width: 100%; text-align: center;">
                                <asp:Button Text="◄" ID="btnPrevUsr" runat="server" />
                                <asp:TextBox Width="30px" style="text-align: center;" Text="1" ID="txtPageNumUsr" AutoPostBack="True" runat="server"></asp:TextBox>
                                <asp:Button Text="►" ID="btnNextUsr" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlOrganizations" runat="server" Visible="false">
                        <div style="width: 100%; text-align: center;">
                            <asp:Button ID="btnOrgQueue" runat="server" Text="Organization approval queue" />
                            <asp:Button ID="btnAllOrg" runat="server" Text="Organizations list" />
                            <asp:Button ID="btnActiveOrg" runat="server" Text="Active organizations list" />
                            <asp:Button ID="btnRejectedOrg" runat="server" Text="Rejected Organizations list" />
                            <br />
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search text"></asp:TextBox>
                            <asp:Button ID="btnSearchName" runat="server" Text="Search by name" />
                            <br />
                            <asp:RadioButtonList ID="radioSort" EnableViewState="true" runat="server" CssClass="radioButtonList" RepeatDirection="Horizontal" AutoPostBack="True">
                                
                                <asp:ListItem Selected="True" Value="OrganizationName">A-&gt;Z &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="*OrganizationName">Z-&gt;A&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="*CreationDate">New-&gt;Old&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="CreationDate">Old-&gt;New&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="MonthlyPoints">Monthly budget&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="*MonthlyPoints">Monthly budget descending&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div style="width: 100%; min-height: 30px; background-color: antiquewhite;">
                            <asp:Label ID="lblOrgs" runat="server"></asp:Label>
                            <br />
                            <div style="width: 100%; text-align: center;">
                                <asp:Button Text="◄" ID="btnPrev" runat="server" />
                                <asp:TextBox Width="30px" style="text-align: center;" Text="1" ID="txtPageNum" AutoPostBack="True" runat="server"></asp:TextBox>
                                <asp:Button Text="►" ID="btnNext" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </td><td style="width: 10%;"></td>
        </tr>
    </table>
</asp:Content>

