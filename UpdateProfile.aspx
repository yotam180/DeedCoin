<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UpdateProfile.aspx.vb" Inherits="UpdateProfile" ValidateRequest="false" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
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
        };

        $(document).ready(load);
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 20%; text-align: center;"></td><td>
                <br />
                <h2>Want to change something?</h2>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox placeholder="Username" CssClass="large_txt" style="font-size: 17px;" ID="txtUsername" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox type="password" placeholder="Current password" CssClass="large_txt" style="font-size: 17px;" ID="txtCurrentPassword" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="2"></asp:TextBox>
                        <br />
                        <asp:TextBox type="password" placeholder="Password (Leave empty if you don't wish to change)" CssClass="large_txt" style="font-size: 17px;" ID="txtPassword" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="2"></asp:TextBox>
                        <br />
                        <asp:TextBox type="password" placeholder="Confirm Password" CssClass="large_txt" style="font-size: 17px;" ID="txtPasswordConfirm" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="3"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="First Name" CssClass="large_txt" style="font-size: 17px;" ID="txtFirstName" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TextMode="SingleLine" TabIndex="4"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Last Name" CssClass="large_txt" style="font-size: 17px;" ID="txtLastName" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TextMode="SingleLine" TabIndex="4"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Email" CssClass="large_txt" style="font-size: 17px;" ID="txtEmail" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TextMode="Email" TabIndex="4"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="About you... (Where do you live, what do you like, what are your interests?)" CssClass="large_txt aboutTxt" style="font-size: 17px;" ID="txtAbout" runat="server" Width="70%" Height="93px" TextMode="MultiLine" Font-Bold="True" AutoPostBack="True" TabIndex="5"></asp:TextBox>
                        <br />
                        <p id="lblCharCount" style="display: none; color: black;"></p>
                        <br />
                        <asp:TextBox placeholder="Address/Location (ex. HaZafon, Israel)" CssClass="large_txt" style="font-size: 17px;" ID="txtAddress" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TextMode="Email" TabIndex="6"></asp:TextBox>
                        <br />
                        <asp:Label runat="server" ID="lblAddress" Visible="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lblError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:LinkButton ID="btnRegister" runat="server" TabIndex="7">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Update</div>
                </asp:LinkButton>
            </td><td style="width: 20%;"></td>
        </tr>
    </table>
</asp:Content>
