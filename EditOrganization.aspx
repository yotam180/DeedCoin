<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditOrganization.aspx.vb" Inherits="EditOrganization" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var load = function () {
            $(".aboutTxt").bind('input propertychange', function() {
                if ($(this).val().length < 30 && $(this).val() !== "") {
                    $("#lblCharCount").show();
                    $("#lblCharCount").text((30 - $(this).val().length).toString() + " chars to go!");
                } else {
                    $("#lblCharCount").hide();
                }
            });
            $("#ContentPlaceHolder1_fileImage").change(function () {
                readURL(this);
            });
        };

        function readURL(input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#ContentPlaceHolder1_Image1').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

        $(document).ready(load);
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 10%; text-align: center;"></td ><td style="width: 40%; vertical-align: top;">
                <h2>Edit</h2>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox placeholder="Organiation name" CssClass="large_txt" style="font-size: 17px;" ID="txtOrgName" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Monthly users (how many people does your organization serve?)" CssClass="large_txt" style="font-size: 17px;" ID="txtMonthlyUsers" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Address (be as accurate as you can)" CssClass="large_txt" style="font-size: 17px;" ID="txtAddress" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:Label runat="server" ID="lblAddress" Visible="False"></asp:Label>
                        <br />
                        <asp:TextBox placeholder="Organization Description" CssClass="large_txt" style="font-size: 17px;" ID="txtDescription" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1" Height="144px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Opening hours" CssClass="large_txt" style="font-size: 17px;" ID="txtOpeningHours" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Audience" CssClass="large_txt" style="font-size: 17px;" ID="txtAudience" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lblError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:LinkButton ID="btnUpdate" runat="server" TabIndex="7">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Submit</div>
                </asp:LinkButton>
            </td>
            <td style="width: 40%; vertical-align: top; text-align: center;">
                <h2>Manage</h2>
                <asp:Panel ID="pnlOrganizationOwner" runat="server">
                <table style="width: 90%; table-layout: fixed; text-align: left; padding: 3px;" border="0">
                    <tr>
                        <td>
                            <b>DeedCoins:</b>
                        </td><td>
                            <asp:Label ID="lblCoins" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>DeedCoins per month:</b>
                        </td><td>
                            <asp:Label ID="lblMonthlyCoins" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Interactions this month:</b>
                        </td><td>
                            <asp:Label ID="lblMonthlyInteractions" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Interactions:</b>
                        </td><td>
                            <asp:Label ID="lblInteractions" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>DeedCoins spent:</b>
                        </td><td>
                            <asp:Label ID="lblSpent" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <br /><br />
                <asp:Panel ID="pnlAdministrator" runat="server">
                    <h1>Administrate</h1>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <b>Monthly Coins</b><br />
                        <asp:TextBox placeholder="Monthly Coins" CssClass="large_txt" style="text-align: center; font-size: 17px;" ID="txtMonthlyCoins" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <b>Current Coin balance</b><br />
                        <asp:TextBox placeholder="Current Coin balance" CssClass="large_txt" style="text-align: center; font-size: 17px;" ID="txtCurrentBalance" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lblError2" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:LinkButton ID="btnUpdate2" runat="server" TabIndex="7">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Update</div>
                </asp:LinkButton>
                <br />
                <asp:LinkButton ID="btnBlock" runat="server" TabIndex="7">
                <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative; background-color: red;">Deactivate organization</div>
                </asp:LinkButton>
                </asp:Panel>
            </td><td style="width: 10%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>