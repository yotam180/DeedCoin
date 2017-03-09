<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddOrganization.aspx.vb" Inherits="AddOrganization" MasterPageFile="~/MasterPage.master" %>

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
            <td style="width: 20%; text-align: center;"></td><td>
                <br />
                <h2>Start a new place to contribute</h2>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox placeholder="Organiation name" CssClass="large_txt" style="font-size: 17px;" ID="txtOrgName" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Monthly users (how many people does your organization serve?)" CssClass="large_txt" style="font-size: 17px;" ID="txtMonthlyUsers" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Request Message" CssClass="large_txt" style="font-size: 17px;" ID="txtRequestMessage" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1" Height="144px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Address (be as accurate as you can)" CssClass="large_txt" style="font-size: 17px;" ID="txtAddress" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:Label runat="server" ID="lblAddress" Visible="False"></asp:Label>
                        <br />
                        <asp:TextBox placeholder="Organization Description" CssClass="large_txt" style="font-size: 17px;" ID="txtDescription" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1" Height="144px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Image ID="Image1" runat="server" Height="200px" Width="200px" ></asp:Image><br />
                Upload Organization Image/Logo: <asp:FileUpload ID="fileImage" runat="server" />
                <br /><br />
                <asp:Label ID="lblError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:LinkButton ID="btnRegister" runat="server" TabIndex="7">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Submit</div>
                </asp:LinkButton>
            </td><td style="width: 20%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>