<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddJob.aspx.vb" Inherits="AddJob" MasterPageFile="~/MasterPage.master" %>


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
                <h2>Hire</h2>
                                <asp:Label ID="lblError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox placeholder="Job title" CssClass="large_txt" style="font-size: 17px;" ID="txtGigName" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <strong>Organization: </strong><asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true">
                        </asp:DropDownList><br />
                        <asp:Image ID="imgOrg" runat="server" Height="100px" /><br />
                        
                        <asp:TextBox placeholder="Short description" CssClass="large_txt" style="font-size: 17px;" ID="txtShortDescription" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br /><br />
                        <asp:TextBox placeholder="Description (markdown friendly)" CssClass="large_txt" style="font-size: 17px;" ID="txtDescription" runat="server" Width="100%" Font-Bold="True" AutoPostBack="True" TabIndex="1" Height="300px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Salary (in deedCoins)" CssClass="large_txt" style="font-size: 17px;" ID="txtPrice" runat="server" Width="100%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Video URL (not required)" CssClass="large_txt" style="font-size: 17px;" ID="txtVideoURL" runat="server" Width="100%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <asp:TextBox placeholder="Image URLs (one per line)" CssClass="large_txt" style="font-size: 17px;" ID="txtImages" runat="server" Width="100%" Font-Bold="True" AutoPostBack="True" TabIndex="1" Height="60px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        * First image URL will be the featured image of your job.
                        
                        <br /><br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:LinkButton ID="btnAdd" runat="server" TabIndex="7">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Submit</div>
                </asp:LinkButton>
            </td><td style="width: 20%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>